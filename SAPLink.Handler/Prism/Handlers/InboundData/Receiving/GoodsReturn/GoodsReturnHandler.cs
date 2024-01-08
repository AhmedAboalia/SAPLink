using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP.Documents;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.EF;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Integration;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.SAP.Application;
using Serilog;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsReceiptPo;

public class GoodsReturnHandler
{
    private static UnitOfWork UnitOfWork;

    private readonly ReceivingService _receivingService;
    private readonly ItemsHandler _itemsHandler;
    private static ItemsService _itemsService;
    private static DepartmentService _departmentServicess;
    private static VendorsService _vendorsService;
    private static Clients _client;
    private static ILogger _loger;
    public List<Store>? Stores;


    public GoodsReturnHandler(UnitOfWork unitOfWork, Clients client)
    {
        UnitOfWork = unitOfWork;
        _receivingService = new ReceivingService(client);

        _departmentServicess = new(client);
        _vendorsService = new(client);
        _client = client;
        _itemsService = new ItemsService(client, _departmentServicess, _vendorsService);
        _itemsHandler = new ItemsHandler(unitOfWork, _itemsService, client);
        _loger = Helper.CreateLoggerConfiguration("Goods Return", "Handler", LogsTypes.InboundData);

    }
    public async IAsyncEnumerable<RequestResult<Goods>> SyncAsync(string filter = "")
    {
        string logStatus = null;
        string logMessage = null;

        var outList = new List<Goods>();
        var goodsReturns = new List<Goods>();
        var result = new RequestResult<Goods>();

        if (filter.IsNullOrEmpty())
            filter += " WHERE (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";

        goodsReturns = GetGoodsReturn(filter);

        if (goodsReturns.Count > 0)
        {
            foreach (var goodsReturn in goodsReturns)
            {
                //var store = await _receivingService.GetStore(goodsReceiptPo.WarehouseCode);
                //var storeSid = store.Sid;

                var store = await GetStore(goodsReturn);
                var IsLocationChanged = _receivingService.ChangeLocation(store.Sid);
                var receiving = await _receivingService.GenerateVoucherSid(store.Sid);
                        
                foreach (var line in goodsReturn.Lines)
                {
                    var itemResult = await _itemsService.GetByCodeAsync(line.ItemCode);
                    var product = itemResult.EntityList.FirstOrDefault();

                    if (product != null)
                    {
                        var itemPayload = _receivingService.CreateAddConsolidateItemPayload(product, receiving.Sid, line);

                        //logMessage += $"\r\n SID: {receiving.Sid} \r\n {itemPayload} \r\n";
                        var itemResponse = await _receivingService.AddConsolidateItem(itemPayload, receiving.Sid);

                        if (itemResponse.StatusCode == HttpStatusCode.OK)
                        {
                            var item = JsonConvert.DeserializeObject<OdataPrism<Goods>>(itemResponse.Content).Data
                                .ToList();

                            if (item.Any())
                            {
                                logMessage += $"Goods Return: {goodsReturn.DocEntry} >> Line item ({line.ItemCode} : {line.ItemName}) is Added.";

                                LogInformation(logMessage);
                                result.Message = logMessage + "\r\n";
                                yield return result;
                            }
                            else
                            {
                                logMessage = $"Exception: Cannot Add or Sync Receiving (Return). SID: ({receiving.Sid}). \r\n" +
                                             $"Goods Return Line item ({line.ItemCode} : {line.ItemName}) \r\n" +
                                             $"Response Content: {itemResponse.Content}";

                                LogError(logMessage);
                                result.Message = logMessage;
                                yield return result;
                            }
                        }
                    }
                    else
                    {
                        var message = $"Item Not found {line.ItemCode} : {line.ItemName} will try to add it.";
                        logMessage += $"\r\n{message}\n\r";

                        LogInformation(logMessage);

                        result.Message = logMessage;
                        yield return result;

                        var filterQuery = $" AND T0.ItemCode = '{line.ItemCode}'";
                        _itemsHandler.SyncAsync(filterQuery);
                    }
                }
                //var AddGrpoTrackingNumAndNote = _receivingService.AddGrpoTrackingNumAndNote(GoodsReceiptPO.DocNum,receiving.Sid,receiving.RowVersion, GoodsReceiptPO.Remarks);
                var goodsReturnsReceiving = await _receivingService.GetReceiving(receiving);
                var receivingResponse =  await _receivingService.ChangeReceivingToReturn(goodsReturnsReceiving.Sid, goodsReturnsReceiving.RowVersion);

                var rowVersion = int.Parse(goodsReturnsReceiving.RowVersion) + 1;
                //Thread.Sleep(500);
                var response = await _receivingService.AddReceiving(receiving, rowVersion.ToString(), goodsReturn.DocNum, goodsReturn.Remarks, store.Sid);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var goodsReceiptPos = JsonConvert.DeserializeObject<OdataPrism<Goods>>(response.Content).Data.ToList();

                    if (goodsReceiptPos.Any())
                    {
                        var updated = await UpdateGrpo(goodsReturn.DocEntry, receiving.Sid);


                        outList.Add(goodsReturn);

                        var message = $"Goods Return No. ({goodsReturn.DocEntry}) - SID: ({receiving.Sid}) is Added.";

                        if (updated)
                            message += $"\r\nSuccessfully Update Sync Flag for Goods Return No. ({goodsReturn.DocEntry}).";

                        else
                            message += $" Can`t Updated Sync Flag for Goods Return No. ({goodsReturn.DocEntry}).";

                        logMessage += $"{message}\r\n";
                        logStatus = message;

                        LogInformation(message);


                        yield return new RequestResult<Goods>(Enums.StatusType.Success, logMessage, logStatus, outList, response);

                        SyncEntityService.UpdateSyncEntityDate(UnitOfWork, Enums.UpdateType.InitialGoodsReceiptPO);
                        SyncEntityService.UpdateSyncEntityDate(UnitOfWork, Enums.UpdateType.SyncGoodsReceiptPO);
                    }
                }
                else
                {
                    logStatus = $"Cannot Add or Sync Receiving (Return). SID: ({receiving.Sid}).";
                    logMessage = $"{logStatus}\r\n" +
                                 $"Response Content:\r\n" +
                                 $"{response.Content}";

                    LogError(logMessage);

                    yield return new RequestResult<Goods>(Enums.StatusType.Failed, logMessage, logStatus, outList, response);

                }
            }
        }
        else
        {
            logMessage = "There is no Goods Returns available to be synced, or may it flagged as synced to prism.";
            logStatus = "Status: there is no Goods Returns available to be synced, or it flagged as synced to prism.";

            _loger.Information(logMessage);
            yield return new RequestResult<Goods>(Enums.StatusType.Success, logMessage, logStatus, outList, new RestResponse());
        }
    }



    private async Task<Store> GetStore(Goods good)
    {
        Store store = null;
        try
        {
            string storeCode = good.WarehouseCode != null
                ? good.WarehouseCode
                : good.Lines.Select(x => x.WarehouseCode).FirstOrDefault();

            store = await _receivingService.GetStore(storeCode);
        }
        catch (Exception e)
        {
            _loger.Error(e, "Cant find Store Code.");
        }

        return store;
    }
    public static async Task<bool> UpdateGrpo(string docCode, string Sid)
    {
        var documents = (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oPurchaseReturns);

        if (documents.GetByKey(Convert.ToInt32(docCode))) // Retrieve the GRPO by its Doc code
        {
            // Update the field value
            documents.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";
            documents.UserFields.Fields.Item("U_PrismSid").Value = Sid;

            // Update the GRPO in the database
            int updateResult = documents.Update();

            return updateResult == 0
                ? Task.CompletedTask.IsCompletedSuccessfully
                : Task.CompletedTask.IsFaulted;
        }
        return false;
    }

    public List<Goods> GetGoodsReturn(string filter = "")
    {
        var goodsReceiptPOs = new List<Goods>();
        try
        {
            var query = @"SELECT 
                        T0.[DocEntry], 
                        T0.[DocNum], 
                        T0.[CardCode], 
                        T0.[CardName],  
                        T0.[Comments],
                        T0.[U_WhsCode]
                            FROM ORPD T0 ";


            if (filter.IsHasValue())
            {
                query += filter;
            }
            if (!ClientHandler.Company.Connected)
            {
                ClientHandler.InitializeClientObjects(_client, out _, out _);
            }

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(query);



            for (var i = 0; i < oRecordSet.RecordCount; i++)
            {
                var grpo = new Goods();
                grpo.DocEntry = oRecordSet.Fields.Item("DocEntry").Value.ToString();
                grpo.DocNum = oRecordSet.Fields.Item("DocNum").Value.ToString();
                grpo.CardCode = oRecordSet.Fields.Item("CardCode").Value.ToString();
                grpo.CardName = oRecordSet.Fields.Item("CardName").Value.ToString();
                grpo.WarehouseCode = oRecordSet.Fields.Item("U_WhsCode").Value.ToString();
                grpo.Remarks = oRecordSet.Fields.Item("Comments").Value.ToString();

                grpo.Lines = GetGoodsReturnLines(grpo.DocEntry);

                goodsReceiptPOs.Add(grpo);

                oRecordSet.MoveNext();
            }
        }
        catch (Exception e)
        {

        }

        return goodsReceiptPOs;
    }

    public List<Line> GetGoodsReturnLines(string docEntry = "")
    {
        var query = @$"SELECT 
                        T0.[DocEntry],
                        T0.[ItemCode], 
                        T0.[Dscription], 
                        T0.[U_QTY], 
                        T0.[Quantity], 
                        T0.[Price],
                        T0.[WhsCode]
                            FROM RPD1 T0  
                                WHERE T0.[DocEntry] = '{docEntry}'";

        //UPDATE PDN1  SET PDN1.U_QTY =  (SELECT T0.[Quantity] FROM PDN1 T0 WHERE T0.[DocEntry] = PDN1.DocEntry and T0.LineNum = PDN1.LineNum) 
        if (!ClientHandler.Company.Connected)
        {
            ClientHandler.InitializeClientObjects(_client, out _, out _);
        }

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        oRecordSet.DoQuery(query);

        var lines = new List<Line>();

        for (var i = 0; i < oRecordSet.RecordCount; i++)
        {
            var line = new Line();
            line.DocEntry = oRecordSet.Fields.Item("DocEntry").Value.ToString();
            line.ItemCode = oRecordSet.Fields.Item("ItemCode").Value.ToString();
            line.ItemName = oRecordSet.Fields.Item("Dscription").Value.ToString();
            line.Quantity = Convert.ToDecimal(oRecordSet.Fields.Item("U_QTY").Value.ToString());
            line.Price = Convert.ToDecimal(oRecordSet.Fields.Item("Price").Value.ToString());
            line.WarehouseCode = oRecordSet.Fields.Item("WhsCode").Value.ToString();

            lines.Add(line);

            oRecordSet.MoveNext();
        }

        return lines;
    }

    private void LogInformation(string message)
    {
        _loger.Information(message);
    }

    private void LogError(string message)
    {
        _loger.Error(message);
    }
}