﻿using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.StockManagement;
using SAPLink.Core.Models.System;
using SAPLink.Handler.SAP.Application;
using Serilog;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryTransfer;

public partial class InventoryTransferHandler
{
    private static Clients _client;
    private static ILogger _loger;


    public InventoryTransferHandler(Clients client)
    {
        _client = client;
        _loger = Helper.CreateLoggerConfiguration("Verified Vouchers - (Inventory Transfer)", "Handler", LogsTypes.OutboundData);

    }
    public async IAsyncEnumerable<RequestResult<VerifiedVoucher>> SyncVerifiedVoucher(VerifiedVoucher verifiedVoucher)
    {
        var result = new RequestResult<VerifiedVoucher>();

        var oStockTransfer = (SAPbobsCOM.StockTransfer)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oStockTransfer);

        oStockTransfer.TaxDate = Convert.ToDateTime(verifiedVoucher.CreatedDate);
        oStockTransfer.DocDate = Convert.ToDateTime(verifiedVoucher.CreatedDate);
        oStockTransfer.DueDate = Convert.ToDateTime(verifiedVoucher.CreatedDate);

        oStockTransfer.FromWarehouse = verifiedVoucher.SlipStoreCode;
        oStockTransfer.Series = int.Parse(GetSeriesCode(verifiedVoucher.SlipStoreCode, out string message));
        oStockTransfer.Comments = $"Verified Voucher No. : {verifiedVoucher.Vouno} - Voucher Sid: {verifiedVoucher.Sid} - Slip No. : {verifiedVoucher.Slipno}\r\n" +
                                    $"\r\n<< Inventory Transfers >> " +
                                    $"\r\nFrom Warehouse ({verifiedVoucher.SlipStoreCode})" +
                                    $"\r\nTo Warehouse ({verifiedVoucher.Storecode}).";

        foreach (var item in verifiedVoucher.Recvitem)
        {
            oStockTransfer.Lines.ItemCode = item.Alu;
            Decimal.TryParse(item.SalesPerUnitFactor, out var factor);
            if (factor > 0)
                oStockTransfer.Lines.UserFields.Fields.Item("Quantity").Value = (item.Qty * factor).ToString();
            else
                oStockTransfer.Lines.UserFields.Fields.Item("Quantity").Value = item.Qty.ToString();
            oStockTransfer.Lines.Price = item.Price;
            oStockTransfer.Lines.WarehouseCode = verifiedVoucher.Storecode;
            oStockTransfer.Lines.Add();
        }

        int results = oStockTransfer.Add();

        // Check for errors
        if (results == 0)
        {
            // No errors occurred, retrieve the DocEntry of the newly added stock transfer.
            string docEntry = ClientHandler.Company.GetNewObjectKey();

            // Use the DocEntry to retrieve the Stock Transfer and then fetch its DocNum.
            oStockTransfer.GetByKey(int.Parse(docEntry));

            int docNum = oStockTransfer.DocNum;


            result.Message += $"SAP IT ({docNum}) created. " +
                              $"- Voucher No. : {verifiedVoucher.Vouno} - Sid: {verifiedVoucher.Sid} - Slip No. : {verifiedVoucher.Slipno} - " +
                              $"From ({verifiedVoucher.SlipStoreCode}) To ({verifiedVoucher.Storecode}).";

            SetInvoiceAsSynced(verifiedVoucher.Sid, docNum.ToString());
            result.Message += $"\r\nSAP IT ({docNum}) is Synced.";

            _loger.Information(result.Message);
            result.Status = Enums.StatusType.Success;
            result.EntityList.Add(verifiedVoucher);
            yield return result;
        }
        else
        {
            ClientHandler.Company.GetLastError(out results, out var errorMessage);

            _loger.Error(result.Message);
            //result.Message += $"\r\nFailed to create stock transfer. \r\nError {result.Message}: {errorMessage}";

            result.Message += $"Faild to Create SAP IT." +
                            $"- Voucher No. : {verifiedVoucher.Vouno} - Sid: {verifiedVoucher.Sid} - Slip No. : {verifiedVoucher.Slipno} - " +
                            $"From ({verifiedVoucher.SlipStoreCode}) To ({verifiedVoucher.Storecode}). \r\nError {result.Message}: {errorMessage}";
            
            _loger.Error(result.Message);

            result.Status = Enums.StatusType.Failed;
            yield return result;
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
                                     WHERE T0.[ObjectCode] = '67' AND T1.WhsCode = '{storeCode}'";

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

    private static void SetInvoiceAsSynced(string invoiceSid, string docEntry)
    {
        var query = $"UPDATE OWTR SET U_SyncToPrism = 'Y', U_PrismSid = '{invoiceSid}' WHERE DocNum = '{docEntry}'";
        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

        oRecordSet.DoQuery(query);
    }
}