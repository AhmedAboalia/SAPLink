using SAPLink.EF;
using SAPLink.Core.Models;
using SAPLink.Core.Models.SAP.Documents;
using SAPLink.Core.Models.System;
using SAPbobsCOM;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Connected_Services;
using static SAPLink.Core.Enums;
using Documents = SAPbobsCOM.Documents;
using SAPLink.Handler.Integration;
using SAPLink.Handler.SAP.Application;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using Serilog;
using ServiceLayerHelper.RefranceModels;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Receiving.GoodsIssue;

public class GoodsIssueHandler
{
    private static UnitOfWork UnitOfWork;

    private readonly ReceivingService _receivingService;
    private readonly ItemsHandler _itemsHandler;
    private static ItemsService _itemsService;
    private static DepartmentService _departmentServicess;
    private static VendorsService _vendorsService;
    private static Clients _client;
    private static ILogger _loger;

    public GoodsIssueHandler(UnitOfWork unitOfWork, Clients client)
    {
        UnitOfWork = unitOfWork;
        _receivingService = new ReceivingService(client);

        _departmentServicess = new(client);
        _vendorsService = new(client);
        _client = client;
        _itemsService = new ItemsService(client, _departmentServicess, _vendorsService);
        _itemsHandler = new ItemsHandler(unitOfWork, _itemsService, client);
        _loger = Helper.CreateLoggerConfiguration("Goods Issues (GI)", "Handler", LogsTypes.InboundData);

    }
    public async IAsyncEnumerable<RequestResult<Goods>> SyncAsync(string filter = "")
    {
        string logStatus = null;
        string logMessage = null;

        var outList = new List<Goods>();
        var goodsIssues = GetGoodsIssues(filter);

        if (goodsIssues.Count > 0)
        {
            foreach (var goodsIssue in goodsIssues)
            {
                //var store = await _receivingService.GetStore(goodsIssue.WarehouseCode);
                //var storeSid = store.Sid;
                var store = await GetStore(goodsIssue);

                try
                {
                    var IsLocationChanged = _receivingService.ChangeLocation(store.Sid);
                }
                catch (Exception)
                {

                }


                var receiving = await _receivingService.GenerateVoucherSid(store.Sid);
                foreach (var line in goodsIssue.Lines)
                {
                    var result = await _itemsService.GetByCodeAsync(line.ItemCode);
                    var product = result.EntityList.FirstOrDefault();

                    if (product != null)
                    {
                        var itemPayload = _receivingService.CreateAddConsolidateItemPayload(product, receiving.Sid, line);

                        //logMessage += $"\r\n SID: {receiving.Sid} \r\n {itemPayload} \r\n";
                        var itemRespose = await _receivingService.AddConsolidateItem(itemPayload, receiving.Sid);

                        if (itemRespose.StatusCode == HttpStatusCode.OK)
                        {
                            try
                            {
                                var item = JsonConvert.DeserializeObject<OdataPrism<Goods>>(itemRespose.Content).Data.ToList();

                                if (item.Any())
                                {
                                    logMessage += $"\r\n\r\nGoods Issue (GI) No.: {goodsIssue.DocEntry} >> Line item ({line.ItemCode} : {line.ItemName}) is Added.\r\n";
                                    _loger.Information($"Goods Issue (GI) No.: {goodsIssue.DocEntry} >> Line item ({line.ItemCode} : {line.ItemName}) is Added.");
                                }
                            }
                            catch (Exception e)
                            {
                                logMessage = $"Exception: Cannot Add or Sync Goods Issue (GI). SID: ({receiving.Sid}). \r\n" +
                                             $"GI Line item ({line.ItemCode} : {line.ItemName}) \r\n" +
                                             $"Response Content: {itemRespose.Content}";
                                _loger.Error(logMessage);
                            }
                        }
                        else
                        {
                            logStatus = $"Cannot Add or Sync Goods Issue (GI). SID: ({receiving.Sid}).";
                            logMessage = $"Cannot Add or Sync Goods Issue (GI). SID: ({receiving.Sid}).\r\n" +
                                         $"\r\n\r\nResponse Content:\r\n\r\n" +
                                         $"{itemRespose.Content}";

                            _loger.Information(logMessage);

                            yield return new RequestResult<Goods>(StatusType.Failed, logMessage, logStatus, outList, itemRespose);

                        }
                    }
                    else
                    {
                        var message = $"Item Not found {line.ItemCode} : {line.ItemName} will try to add it.";
                        logMessage += $"\r\n{message}\n\r";

                        _loger.Information(message);

                        var filterQuery = $" AND T0.ItemCode = '{line.ItemCode}'";
                        _itemsHandler.SyncAsync(filterQuery);
                    }
                }
                //var AddGrpoTrackingNumAndNote = _receivingService.AddGrpoTrackingNumAndNote(GoodsReceiptPO.DocNum,receiving.Sid,receiving.RowVersion, GoodsReceiptPO.Remarks);
                var receiving2 = await _receivingService.GetReceiving(receiving);


                var response = await _receivingService.AddReceiving(receiving, receiving2.RowVersion, goodsIssue.DocNum, goodsIssue.Remarks, store.Sid);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var goodsReceiptPos = JsonConvert.DeserializeObject<OdataPrism<Goods>>(response.Content).Data.ToList();

                    if (goodsReceiptPos.Any())
                    {
                        await UpdateGI(goodsIssue.DocEntry, receiving.Sid);

                        outList.Add(goodsIssue);

                        var message = $"Goods Issue (GI) ({goodsIssue.DocNum}) - SID: ({receiving.Sid}) is Added.";
                        logMessage += $"{message}\r\n";
                        logStatus = message;

                        _loger.Information(message);

                        yield return new RequestResult<Goods>(StatusType.Success, logMessage, logStatus, outList, response);

                        SyncEntityService.UpdateSyncEntityDate(UnitOfWork, UpdateType.InitialGoodsReceiptPO);
                        SyncEntityService.UpdateSyncEntityDate(UnitOfWork, UpdateType.SyncGoodsReceiptPO);
                    }
                }
                else
                {
                    logStatus = $"Cannot Add or Sync Goods Issue (GI). SID: ({receiving.Sid}).";
                    logMessage = $"Cannot Add or Sync Goods Issue (GI). SID: ({receiving.Sid}).\r\n" +
                                 $"Response Content:\r\n" +
                                 $"{response.Content}";

                    _loger.Information(logMessage);
                    yield return new RequestResult<Goods>(StatusType.Failed, logMessage, logStatus, outList, response);

                }
            }
        }
        else
        {
            logMessage = "There is no Goods Issues available to be synced, or may it flagged as synced to prism.";
            logStatus = "Status: there is no Goods Issues available to be synced, or it flagged as synced to prism.";

            _loger.Information(logMessage);

            yield return new RequestResult<Goods>(StatusType.Success, logMessage, logStatus, outList, new RestResponse());
        }
    }

    private static async Task<bool> UpdateGI(string docCode, string Sid)
    {
        var businessPartners = (Documents)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oInventoryGenExit);

        if (businessPartners.GetByKey(Convert.ToInt32(docCode))) // Retrieve the GR by its Doc code
        {
            // Update the field value
            businessPartners.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";
            businessPartners.UserFields.Fields.Item("U_PrismSid").Value = Sid;

            // Update the GRPO in the database
            int updateResult = businessPartners.Update();

            return updateResult == 0 ? // Update successful
                Task.CompletedTask.IsCompletedSuccessfully
                : Task.CompletedTask.IsFaulted;
        }
        return false;
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
    public List<Goods> GetGoodsIssues(string filter = "")
    {
        var goodsReceiptPOs = new List<Goods>();
        try
        {
            var query = @"SELECT 
                              T0.[DocEntry],
                              T0.[DocNum], 
                              T0.[DocDate], 
                              T0.[DocDueDate], 
                              T0.[Comments] 
                                        FROM OIGE T0";

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
                //grpo.CardCode = oRecordSet.Fields.Item("CardCode").Value.ToString();
                //grpo.CardName = oRecordSet.Fields.Item("CardName").Value.ToString();
                grpo.Remarks = oRecordSet.Fields.Item("Comments").Value.ToString();

                grpo.Lines = GetGoodsReceiptPOLines(grpo.DocEntry);

                goodsReceiptPOs.Add(grpo);

                oRecordSet.MoveNext();
            }
        }
        catch (Exception e)
        {

        }

        return goodsReceiptPOs;
    }

    public List<Line> GetGoodsReceiptPOLines(string docEntry = "")
    {
        var query = @$"SELECT 
                        T0.[DocEntry],
                        T0.[ItemCode], 
                        T0.[Dscription], 
                        T0.[U_QTY], 
                        T0.[Quantity], 
                        T0.[Price],
                        T0.[WhsCode]
                            FROM IGE1 T0  
                                WHERE T0.[DocEntry] = '{docEntry}'";

        //T0.[U_QTY], T0.[OpenQty]
        //UPDATE IGE1  SET IGE1.U_QTY =  (SELECT T0.[Quantity] FROM IGE1 T0 WHERE T0.[DocEntry] = IGE1.DocEntry and T0.LineNum = IGE1.LineNum) 
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
            line.Quantity = -Convert.ToDecimal(oRecordSet.Fields.Item("U_QTY").Value.ToString());
            line.Price = Convert.ToDecimal(oRecordSet.Fields.Item("Price").Value.ToString());
            line.WarehouseCode = oRecordSet.Fields.Item("WhsCode").Value.ToString();

            lines.Add(line);

            oRecordSet.MoveNext();
        }

        return lines;
    }
}