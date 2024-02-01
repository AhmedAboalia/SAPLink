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

    private static ILogger _loger;

    public CreditMemoHandler(Clients client)
    {
        _client = client;
        _loger = Helper.CreateLoggerConfiguration("AR Credit Memo", "Handler", LogsTypes.OutboundData);

    }
    public async IAsyncEnumerable<RequestResult<SAPInvoice>> AddCreditMemoAsync(List<PrismInvoice> invoicesList)
    {
        var result = new RequestResult<SAPInvoice>();

        if (invoicesList.Count > 0)
        {
            foreach (var invoice in invoicesList)
            {
                var downPaymentDocEntry = GetDownPaymentDocEntry(invoice.RefSaleDocSid, out var message);
                var downPaymentDocNum = ActionHandler.GetStringValueByQuery($"SELECT T0.[DocNum] FROM ODPI T0 WHERE T0.[DocEntry] = {downPaymentDocEntry}");
                //var creditMemoDocNum = ActionHandler.GetStringValueByQuery($"SELECT TOP 1 T0.[DocEntry] FROM ORIN T0 WHERE T0.[U_PrismSid] = '{invoice.Sid}'");
                var arDownPayment = (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oDownPayments);

                if (arDownPayment.GetByKey(int.Parse(downPaymentDocEntry)))
                {
                    if (arDownPayment.DocumentStatus == BoStatus.bost_Open)
                    {

                        var series = GetSeriesCode(invoice.StoreCode, out string message3);

                        var oCreditMemo =
                            (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oCreditNotes);

                        var customerCode = Handler.GetCustomerCodeByStoreCode(invoice.StoreCode, out string message2);

                        oCreditMemo.CardCode = customerCode;
                        oCreditMemo.DocType = BoDocumentTypes.dDocument_Items;
                        oCreditMemo.Series = int.Parse(series);
                        oCreditMemo.DocDate = DateTime.Today;
                        oCreditMemo.TaxDate = DateTime.Today;
                        oCreditMemo.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";
                        oCreditMemo.UserFields.Fields.Item("U_PrismSid").Value = invoice.Sid;
                        oCreditMemo.Comments = $"Based on A/R Down Payment {downPaymentDocNum}.\r\nPrism Return No. {invoice.DocumentNumber}\r\nPrism Return Sid. {invoice.Sid}\r\n";

                        oCreditMemo.Lines.BaseType = (int)BoObjectTypes.oDownPayments;  // 203 for A/R Down Payment
                        oCreditMemo.Lines.BaseEntry = int.Parse(downPaymentDocEntry);  // This should be the DocEntry of the A/R Down Payment
                        oCreditMemo.Lines.BaseLine = arDownPayment.Lines.BaseLine;  // Set BaseLine to line 0 of the A/R Down Payment
                        oCreditMemo.Lines.Add();

                    for (int i = 1; i < arDownPayment.Lines.Count; i++)
                        {
                            oCreditMemo.Lines.SetCurrentLine(i);
                            oCreditMemo.Lines.BaseLine = i; // Note: Using LineNum instead of BaseLine
                            oCreditMemo.Lines.BaseEntry = int.Parse(downPaymentDocEntry);
                            oCreditMemo.Lines.BaseType = (int)BoObjectTypes.oDownPayments;

                            oCreditMemo.Lines.ItemCode = invoice.Items[i].Alu;
                            oCreditMemo.Lines.Quantity = invoice.Items[i].Quantity;
                            oCreditMemo.Lines.Price = invoice.Items[i].Price;

                            oCreditMemo.Lines.Add();
                        }

                        if (oCreditMemo.Add() == 0)
                        {
                            //var resulOutgoing = OutgoingPayment.AddMultiplePaymentsInvoice(invoice, "", customerCode, BoRcptInvTypes.it_DownPayment);
                            //result.Message += $"\r\n {resulOutgoing.Message}";
                            //result.Status = resulOutgoing.Status;
                            
                            if (downPaymentDocNum != "0")
                                result.Message +=
                                       $"\r\nSuccessfully created A/R Credit Memo, Prism Return No. {invoice.DocumentNumber} - Sid. {invoice.Sid} based on A/R Down Payment {downPaymentDocNum}.\r\n";
                            else
                                result.Message +=
                                       $"\r\nSuccessfully created A/R Credit Memo, Prism Return No. {invoice.DocumentNumber} - Sid. {invoice.Sid} based on A/R Down Payment.\r\n";


                            _loger.Information(result.Message);
                        }
                        else
                        {
                            ClientHandler.Company.GetLastError(out var errorCode, out var errorMsg);
                            result.Message += $"Failed to add A/R Credit Memo. Prism Return {invoice.DocumentNumber} Error: {errorMsg}";
                            _loger.Error(result.Message);
                        }
                    }
                    else if (arDownPayment.DocumentStatus == BoStatus.bost_Close)
                    {
                        result.Message += $"[Error]\r\nCant Add A/R Credit Memo\r\nA/R Down Payment No.: {arDownPayment.DocNum} is already closed.";
                        _loger.Warning(result.Message);
                        // Handle or exit, since the A/R Down Payment is closed and cannot be referenced further.
                    }
                }
                else
                {
                    ClientHandler.Company.GetLastError(out var errorCode, out var errorMsg);
                    result.Message += $"\r\nRelated A/R Down Payment not found, Please Sync it Before you try to Sync Return No. ({invoice.DocumentNumber}), (Error: {errorMsg})";
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
    private static void SetCreditMemoAsSynced(string invoiceSid, string docEntry)
    {
        var query = $"UPDATE ORIN SET U_SyncToPrism = 'Y', U_PrismSid = '{invoiceSid}' WHERE DocNum = '{docEntry}'";
        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

        oRecordSet.DoQuery(query);
    }
    public static string GetDownPaymentDocEntry(string prismInvoiceNo, out string message)
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