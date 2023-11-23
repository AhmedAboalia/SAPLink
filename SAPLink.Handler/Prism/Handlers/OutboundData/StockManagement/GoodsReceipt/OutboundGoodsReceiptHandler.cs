using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.System;
using SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Handlers;
using Serilog;
using ServiceLayerHelper.RefranceModels;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;
using SAPInventoryPosting = SAPLink.Core.Models.Prism.StockManagement.InventoryPosting;
using SAPLink.Core.Models.Prism.StockManagement;
using SAPLink.Core.Models.SAP.Sales;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.GoodsReceipt;

public partial class OutboundGoodsReceiptHandler
{
    private static Clients _client;
    private static InvoiceService _invoiceService;
    private readonly ServiceLayerHandler _serviceLayer;
    private static ILogger _loger;

    public OutboundGoodsReceiptHandler(Clients client, InvoiceService invoiceService, ServiceLayerHandler serviceLayer)
    {
        _client = client;
        _invoiceService = invoiceService;
        _serviceLayer = serviceLayer;
        _loger = Helper.CreateLoggerConfiguration("AR Goods Receipt", "Handler", LogsTypes.OutboundData);

    }
    public async IAsyncEnumerable<RequestResult<SAPInventoryPosting>> AddGoodsReceiptAsync(List<SAPInventoryPosting> inventoryPostingList)
    {
        var result = new RequestResult<SAPInventoryPosting>();

        if (inventoryPostingList.Count > 0)
        {
            foreach (var inventoryPosting in inventoryPostingList)
            {
                var oGoodsReceipt = (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);  // Goods Receipt Object

                var series = GetSeriesCode(inventoryPosting.StoreCode, out string message3);

                //var customerCode = Handler.GetCustomerCodeByStoreCode(invoice.StoreCode, out string message2);

                oGoodsReceipt.DocType = BoDocumentTypes.dDocument_Items;
                oGoodsReceipt.Series = int.Parse(series);
                oGoodsReceipt.DocDate = Convert.ToDateTime(inventoryPosting.CreateDate);
                oGoodsReceipt.TaxDate = Convert.ToDateTime(inventoryPosting.CreateDate);
                oGoodsReceipt.DocDueDate = Convert.ToDateTime(inventoryPosting.CreateDate);
                //oGoodsReceipt.Comments = $"Based on Goods Receipt {docEntry}.";

                //oGoodsReceipt.Lines.BaseType = (int)BoObjectTypes.oDownPayments;  // 203 for Goods Receipt
                //oGoodsReceipt.Lines.BaseEntry = int.Parse(docEntry);  // This should be the DocEntry of the Goods Receipt
                //oGoodsReceipt.Lines.BaseLine = arDownPayment.Lines.BaseLine;  // Set BaseLine to line 0 of the Goods Receipt
                //oGoodsReceipt.Lines.Add();
                foreach (var item in inventoryPosting.Adjitem)
                {
                    oGoodsReceipt.Lines.ItemCode = item.Alu;
                    oGoodsReceipt.Lines.Quantity = item.Adjvalue - item.Origvalue;
                    oGoodsReceipt.Lines.Price = item.Price;

                    oGoodsReceipt.Lines.Add();
                }

                if (oGoodsReceipt.Add() == 0)
                {
                    result.Message +=
                        $"\r\nSuccessfully created Goods Receipt {inventoryPosting.Adjno}.\r\n";
                    _loger.Information(result.Message);
                }
                else
                {
                    ClientHandler.Company.GetLastError(out var errorCode, out var errorMsg);
                    result.Message += $"Failed to add Goods Receipt. Error: {errorMsg}";
                    _loger.Error(result.Message);
                }

                yield return result;
            }
        }
        else
        {
            result.Message = "There is no Invoice available to be synced or may it flagged as synced to SAP.";
            result.StatusBarMessage = $"Status: {result.Message}";
            result.Status = Enums.StatusType.NotFound;

            _loger.Information(result.Message);

            yield return result;
        }
    }

    public static string GetCreditMemoDocEntry(string prismInvoiceNo, out string message)
    {
        var docEntry = "";
        message = "";

        try
        {
            var query = @$"SELECT T0.[DocEntry] FROM ODPI T0 WHERE T0.[U_PrismSid] = '{prismInvoiceNo}'";

            CheckCompanyConnection(ref message);

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);

            docEntry = oRecordSet.Fields.Item("DocEntry").Value.ToString();
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        return docEntry;
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

}