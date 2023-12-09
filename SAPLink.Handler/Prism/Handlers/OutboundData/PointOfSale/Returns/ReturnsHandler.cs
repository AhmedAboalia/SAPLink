using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Models.System;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Handlers;
using Serilog;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale.Returns;

public partial class ReturnsHandler
{
    private static Clients _client;
    private readonly ServiceLayerHandler _serviceLayer;
    private static ILogger _loger;

    public ReturnsHandler(Clients client, ServiceLayerHandler serviceLayer)
    {
        _client = client;
        _serviceLayer = serviceLayer;
        _loger = Helper.CreateLoggerConfiguration("Return - (AR Credit Memo)", "Handler", LogsTypes.OutboundData);
    }

    public async IAsyncEnumerable<RequestResult<SAPInvoice>> AddReturnInvoiceAsync(List<PrismInvoice> invoicesList)
    {
        var result = new RequestResult<SAPInvoice>();
        if (!_serviceLayer.Connected())
            _serviceLayer.Connect();

        if (invoicesList.Count > 0)
        {
            foreach (var invoice in invoicesList)
            {
                var customerCode = Handler.GetCustomerCodeByStoreCode(invoice.StoreCode, out string message);
                var series = GetSeriesCode(invoice.StoreCode, out string message2);

                result = _serviceLayer.AddReturnInvoice(invoice, customerCode, series);

                var SAPInvoice = result.EntityList.FirstOrDefault();


                if (result.EntityList.Any())
                {
                    var resultIncoming = new RequestResult<Payment>();
                    if (invoice.Tenders != null && SAPInvoice != null)
                    {
                        resultIncoming = OutgoingPayment.AddMultiplePaymentsInvoice(invoice, SAPInvoice.DocNum, customerCode);
                        result.Message += $"\r\n {resultIncoming.Message}";
                        result.Status = resultIncoming.Status;

                        if (result.Status == Enums.StatusType.Success)
                        {

                            if (result.Status == Enums.StatusType.Success)
                            {
                                SetReturnInvoiceAsSynced(invoice.Sid, SAPInvoice.DocNum);
                                result.Message += $"\r\nSuccessfully Update Sync Flag for the Prism invoice No. {invoice.DocumentNumber} - SAP invoice No. {SAPInvoice.DocNum}.\r\n";
                            }

                            _loger.Information(result.Message);
                        }
                        _loger.Information(result.Message);

                        yield return result;
                    }
                    else
                    {
                        result.Message += "No Available Tenders for Return invoice.";
                        result.Status = Enums.StatusType.NotFound;

                        _loger.Information(result.Message);

                        yield return result;
                    }
                }
                yield return result;
            }
        }
        else
        {
            result.Message = "There is no Return Invoice available to be synced or may it flagged as synced to SAP.";
            result.StatusBarMessage = $"Status: {result.Message}";
            result.Status = Enums.StatusType.NotFound;

            _loger.Information(result.Message);

            yield return result;
        }
    }
    private static void SetReturnInvoiceAsSynced(string invoiceSid, string docEntry)
    {
        var query = $"UPDATE ORIN SET U_SyncToPrism = 'Y', U_PrismSid = '{invoiceSid}' WHERE DocNum = '{docEntry}'";
        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

        oRecordSet.DoQuery(query);
    }

    public static string GetSeriesCode(string storeCode, out string message)
    {
        var cardCode = "";
        message = "";

        try
        {
            var query = @$"SELECT T0.[Series]
                            FROM NNM1 T0 
	                            INNER JOIN OWHS T1 ON T0.[SeriesName] = T1.[Street]
                                     WHERE T0.[ObjectCode] = '14' AND T1.WhsCode = '{storeCode}'";

            CheckCompanyConnection(ref message);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            cardCode = oRecordSet.Fields.Item("Series").Value.ToString();
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        return cardCode;
    }
 
    private static void CheckCompanyConnection(ref string message)
    {
        if (!ClientHandler.Company.Connected)
        {
            ClientHandler.InitializeClientObjects(_client, out var code, out var errorMessage);

            message = code != 0
                ? $"Error :{code}  : {errorMessage}"
                : "Not Connected to Database.";
        }
    }
}