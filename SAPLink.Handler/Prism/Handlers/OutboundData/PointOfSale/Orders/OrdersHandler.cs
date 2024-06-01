using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Models.System;
using SAPLink.EF;
using SAPLink.Handler.SAP.Handlers;
using Serilog;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale.Orders;

public partial class OrdersHandler
{
    private readonly ServiceLayerHandler _serviceLayer;
    private static ILogger _loger;
    private UnitOfWork UnitOfWork;
    public OrdersHandler(ServiceLayerHandler serviceLayer, UnitOfWork unitOfWork)
    {
        _serviceLayer = serviceLayer;
        UnitOfWork = unitOfWork;
        _loger = Helper.CreateLoggerConfiguration("Customer Order - (Incoming Payment)", "Handler", LogsTypes.OutboundData);
    }
    public async IAsyncEnumerable<RequestResult<SAPInvoice>> AddOrdersAsync(List<PrismInvoice> invoicesList)
    {
        var result = new RequestResult<SAPInvoice>();
        if (!_serviceLayer.Connected())
            _serviceLayer.Connect();

        if (invoicesList.Count > 0)
        {
            foreach (var invoice in invoicesList)
            {
                var resultIncoming = new RequestResult<Payment>();
                if (invoice.Tenders != null)
                {

                    resultIncoming = IncomingPayment.AddPayment(invoice, UnitOfWork);
                    result.Message += $"\r\n {resultIncoming.Message}";
                    result.Status = resultIncoming.Status;

                    //if (result.Status == Enums.StatusType.Success)
                    //{
                    //    var updateResult = await _invoiceService.UpdateIsSynced(invoice.Sid, invoice.RowVersion);

                    //    if (updateResult.Status == Enums.StatusType.Success)
                    //        result.Message += $"\r\nUpdate (Synced To SAP) Flag: Yes\r\n";
                    //    else if (updateResult.Status == Enums.StatusType.Failed)
                    //        result.Message += $"\r\nUpdate (Synced To SAP) Flag: No\r\n";
                    //}

                    _loger.Information(result.Message);
                    yield return result;
                }
                else
                {
                    result.Message += "No Available Tenders or Deposits for Order.";
                    result.Status = Enums.StatusType.NotFound;

                    _loger.Error(result.Message);

                    yield return result;
                }
            }
        }
        else
        {
            result.Message = "There is no Order available to be synced or may it flagged as synced to SAP.";
            result.StatusBarMessage = $"Status: {result.Message}";
            result.Status = Enums.StatusType.NotFound;

            _loger.Information(result.Message);

            yield return result;
        }
    }
}