using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Sales;
using SAPLink.Core.Models.SAP.Documents;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Models.System;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Handlers;
using Serilog;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale.Invoice;

public partial class InvoiceHandler
{
    private readonly ServiceLayerHandler _serviceLayer;
    private static ILogger _loger;

    public InvoiceHandler(ServiceLayerHandler serviceLayer)
    {
        _serviceLayer = serviceLayer;
        _loger = Helper.CreateLoggerConfiguration("Sale - (AR Invoice)", "Handler", LogsTypes.OutboundData);

    }
    public async IAsyncEnumerable<RequestResult<SAPInvoice>> AddSalesInvoiceAsync(List<PrismInvoice> invoicesList, Enums.UpdateType updateType, string wholesaleCustomerCode)
    {
        var result = new RequestResult<SAPInvoice>();
        if (!_serviceLayer.Connected())
            _serviceLayer.Connect();

        if (invoicesList.Count > 0)
        {
            foreach (var invoice in invoicesList)
            {
                var customerCode = updateType == Enums.UpdateType.SyncWholesale 
                    ? wholesaleCustomerCode 
                    : Handler.GetCustomerCodeByStoreCode(invoice.StoreCode, out string message);

                var series = Handler.GetSeriesCode(invoice.StoreCode, out string message2);

                result = _serviceLayer.AddSalesInvoice(invoice, customerCode, series, updateType);

                var SAPInvoice = result.EntityList.FirstOrDefault();

                if (result.EntityList.Any())
                {
                    var resultIncoming = new RequestResult<Payment>();
                    if (invoice.Tenders != null && SAPInvoice != null)
                    {

                        if (updateType != Enums.UpdateType.SyncWholesale)
                        {
                            resultIncoming = IncomingPayment.AddMultiplePaymentsInvoice(invoice, SAPInvoice.DocEntry, customerCode);
                            result.Message += $"\r\n{resultIncoming.Message}";
                            result.Status = resultIncoming.Status;
                        }

                        if (result.Status == Enums.StatusType.Success)
                        {
                            SetInvoiceAsSynced(invoice.Sid, SAPInvoice.DocNum);
                            result.Message += $"\r\nSuccessfully Update Sync Flag for the Prism invoice No. {invoice.DocumentNumber} - SAP invoice No. {SAPInvoice.DocNum}.\r\n";
                        }
                   
                        _loger.Information(result.Message);

                        yield return result;
                    }
                    else
                    {
                        result.Message = "No Available Tenders for invoice.";
                        result.Status = Enums.StatusType.NotFound;

                        yield return result;
                    }
                    _loger.Information($"\r\n{result.Message}");
                }
                else
                    yield return result;
            }
        }
        else
        {
            result.Message = "There is no Invoice available to be synced or may it flagged as synced to SAP.";
            result.StatusBarMessage = $"Status: {result.Message}";
            result.Status = Enums.StatusType.NotFound;

            _loger.Information($"\r\n{result.Message}");

            yield return result;
        }
    }

    private static void SetInvoiceAsSynced(string invoiceSid, string docEntry)
    {
        var query = $"UPDATE OINV SET U_SyncToPrism = 'Y', U_PrismSid = '{invoiceSid}' WHERE DocNum = '{docEntry}'";
        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

        oRecordSet.DoQuery(query);
    }
}