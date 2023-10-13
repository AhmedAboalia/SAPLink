using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Sales;
using SAPLink.Core.Models.SAP.Sales;
using SAPLink.Core.Models.System;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.SAP.Handlers;
using Serilog;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;
using SAPInvoice = SAPLink.Core.Models.SAP.Sales.Invoice;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.PointOfSale.Orders;

public partial class CreditMemoHandler
{
    private static Clients _client;
    private static InvoiceService _invoiceService;
    private readonly ServiceLayerHandler _serviceLayer;
    private static ILogger _loger;

    public CreditMemoHandler(Clients client, InvoiceService invoiceService, ServiceLayerHandler serviceLayer)
    {
        _client = client;
        _invoiceService = invoiceService;
        _serviceLayer = serviceLayer;
        _loger = Helper.CreateLoggerConfiguration("AR Credit Memo", "Handler", LogsTypes.OutboundData);

    }
    public async IAsyncEnumerable<RequestResult<SAPInvoice>> AddCreditMemoAsync(List<PrismInvoice> invoicesList)
    {
        var result = new RequestResult<SAPInvoice>();

        if (invoicesList.Count > 0)
        {
            foreach (var invoice in invoicesList)
            {
                var docEntry = GetCreditMemoDocEntry(invoice.RefSaleDocSid, out var message);
                var arDownPayment = (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oDownPayments);

                if (arDownPayment.GetByKey(int.Parse(docEntry)))
                {
                    if (arDownPayment.DocumentStatus == BoStatus.bost_Open)
                    {

                        var series = GetSeriesCode(invoice.StoreCode, out string message3);

                        var oCreditMemo =
                            (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oCreditNotes);

                        //var customerCode = Handler.GetCustomerCodeByStoreCode(invoice.StoreCode, out string message2);

                        oCreditMemo.CardCode = arDownPayment.CardCode;
                        oCreditMemo.DocType = BoDocumentTypes.dDocument_Items;
                        oCreditMemo.Series = int.Parse(series);
                        oCreditMemo.DocDate = DateTime.Today;
                        oCreditMemo.TaxDate = DateTime.Today;
                        oCreditMemo.Comments = $"Based on A/R Down Payment {docEntry}.";

                        //oCreditMemo.Lines.BaseType = (int)BoObjectTypes.oDownPayments;  // 203 for A/R Down Payment
                        //oCreditMemo.Lines.BaseEntry = int.Parse(docEntry);  // This should be the DocEntry of the A/R Down Payment
                        //oCreditMemo.Lines.BaseLine = arDownPayment.Lines.BaseLine;  // Set BaseLine to line 0 of the A/R Down Payment
                        //oCreditMemo.Lines.Add();

                        for (int i = 0; i < arDownPayment.Lines.Count; i++)
                        {
                            arDownPayment.Lines.SetCurrentLine(i);
                            oCreditMemo.Lines.BaseLine = arDownPayment.Lines.LineNum; // Note: Using LineNum instead of BaseLine
                            oCreditMemo.Lines.BaseEntry = int.Parse(docEntry);
                            oCreditMemo.Lines.BaseType = (int)BoObjectTypes.oDownPayments;

                            oCreditMemo.Lines.ItemCode = arDownPayment.Lines.ItemCode;
                            oCreditMemo.Lines.Quantity = arDownPayment.Lines.Quantity;
                            oCreditMemo.Lines.Price = arDownPayment.Lines.Price;

                            if (i != arDownPayment.Lines.Count - 1)
                            {
                                oCreditMemo.Lines.Add();
                            }
                        }

                        if (oCreditMemo.Add() == 0)
                        {
                            result.Message +=
                                $"\r\nSuccessfully created A/R Credit Memo based on A/R Down Payment {arDownPayment.DocNum}.\r\n";
                            _loger.Information(result.Message);
                        }
                        else
                        {
                            ClientHandler.Company.GetLastError(out var errorCode, out var errorMsg);
                            result.Message += $"Failed to add A/R Credit Memo. Error: {errorMsg}";
                            _loger.Error(result.Message);
                        }
                    }
                    else if (arDownPayment.DocumentStatus == BoStatus.bost_Close)
                    {
                        result.Message += $"A/R Down Payment with Doc Entry: {docEntry} is already closed.";
                        _loger.Warning(result.Message);
                        // Handle or exit, since the A/R Down Payment is closed and cannot be referenced further.
                    }
                }
                else
                {
                    ClientHandler.Company.GetLastError(out var errorCode, out var errorMsg);
                    result.Message += $"A/R Down Payment with Doc Entry: {docEntry} not found. Error: {errorMsg}";
                    _loger.Warning(result.Message);
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