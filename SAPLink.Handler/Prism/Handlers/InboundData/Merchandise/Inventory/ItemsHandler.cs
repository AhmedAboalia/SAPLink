using SAPLink.EF;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Inventory.Products;
using SAPLink.Core.Models.SAP.MasterData.Items;
using SAPLink.Core.Models.System;
using SAPbobsCOM;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Integration;
using SAPLink.Handler.SAP.Application;
using Serilog;
using ServiceLayerHelper.RefranceModels;
using SAPLink.Core.Models.SAP.MasterData.BusinessPartners;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory
{
    public partial class ItemsHandler
    {
        private static UnitOfWork _unitOfWork;
        private static ItemsService _itemsService;
        private static Clients _client;
        private readonly Credentials _credentials;
        private readonly Subsidiaries _subsidiary;
        private Logger<ItemMasterData> _logger;
        private static ILogger _loger;
        public ItemsHandler(UnitOfWork unitOfWork, ItemsService itemsService, Clients client)
        {
            _unitOfWork = unitOfWork;
            _itemsService = itemsService;
            _client = client;
            _credentials = _client.Credentials.FirstOrDefault();
            _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
            _loger = Helper.CreateLoggerConfiguration("Inventory (Items Master Data)", "Handler", LogsTypes.InboundData);
        }
        public async IAsyncEnumerable<RequestResult<ItemMasterData>> SyncAsync(string filter)
        {
            var items = new List<ItemMasterData>();
            var itemsOut = new List<ItemMasterData>();

            if (filter.IsNullOrEmpty())
                items = GetItems("AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')");
            else
                items = GetItems(filter);

            if (items.Count > 0)
            {
                for (var index = 0; index < items.Count; index++)
                {
                    var item = items[index];

                    var remainingNo = items.Count  - index;
                    var remaining = remainingNo == 0 
                        ? "No Remaining Items" 
                        : $"Remaining ({remainingNo} of {items.Count})";

                    var checkResult = await _itemsService.GetByCodeAsync(item.ItemCode);

                    _loger.Information("-----------------------------------------------------------------");

                    if (checkResult.Status == Enums.StatusType.Success && checkResult.EntityList.Count > 0)
                    {
                        var product = checkResult.EntityList.FirstOrDefault();


                        var requestBody = await _itemsService.CreateUpdatePayload(item, Convert.ToInt64(product.Sid));
                        var resultSync = await _itemsService.Sync(requestBody);

                        if (resultSync.Response.StatusCode == HttpStatusCode.OK)
                        {
                            var products = JsonConvert
                                .DeserializeObject<OdataPrism<Product>>(resultSync.Response.Content).Data.ToList();

                            if (products.Any())
                            {
                                //await UpdateItem(item.ItemCode);
                                UpdateItem(item.ItemCode);
                                var message = $"Item (Code: {item.ItemCode}, Name: {item.ItemName}) is updated and synced.";
                                var statusMessage =
                                    $"Item (Code: {item.ItemCode}, Name: {item.ItemName}) is updated and synced, {remaining}.";
                               
                                _loger.Information(message);

                                var itemc = new ItemMasterData
                                {
                                    ItemCode = item.ItemCode,
                                    ItemName = item.ItemName,
                                    ForeignName = item.ForeignName,
                                    AveragePrice = item.AveragePrice,
                                    SalesPrice = item.SalesPrice,
                                    BarCode = item.BarCode,
                                };

                                itemsOut.Add(itemc);


                                yield return new RequestResult<ItemMasterData>(Enums.StatusType.Success, $"{remaining}.\r\n" +
                                    $"{message}",
                                    statusMessage, itemsOut, resultSync.Response);
                                _loger.Information(message);
                                SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialItems);
                                SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncItems);
                            }
                        }
                        else
                        {
                            var status = $"Cannot Sync Item (Code: {item.ItemCode}, Name: {item.ItemName})";
                            var message = $"\r\nCannot Add or Sync Item.\r\n" +
                                          $"Request Body:\r\n" +
                                          $"{requestBody.PrettyJson()}\r\n\r\n" +
                                          $"Response Content:\r\n" +
                                          $"{resultSync.Response.Content.PrettyJson()} \r\n";

                            _loger.Error(message, "Error occurred.");

                            var requestResult = new RequestResult<ItemMasterData>(
                                Enums.StatusType.Failed, $"{remaining}.\r\n" +
                                                         $"{message}", status, itemsOut, resultSync.Response);

                            //_logger = new Logger<ItemMasterData>(Enums.StatusType.Failed, requestBody, message);

                            //_unitOfWork.ItemsLog.Add(_logger);
                            //_unitOfWork.SaveChanges();

                            yield return requestResult;
                        }
                    }
                    else
                    {
                        var requestBody = await _itemsService.CreateEntityPayload(item);
                        var resultItemSync = await _itemsService.Sync(requestBody);

                        if (resultItemSync.Response.StatusCode == HttpStatusCode.OK)
                        {
                            var products = JsonConvert
                                .DeserializeObject<OdataPrism<Product>>(resultItemSync.Response.Content).Data.ToList();

                            if (products.Any())
                            {
                                //await UpdateItem(item.ItemCode);
                                UpdateItem(item.ItemCode);

                                var message = $"Item (Code: {item.ItemCode}, Name: {item.ItemName}) is Added, {remaining}.";
                                var status = $"Item (Code: {item.ItemCode}, Name: {item.ItemName}) is Added, {remaining}.";
                                SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialItems);
                                SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncItems);
                                _loger.Information(message);

                                var itemc = new ItemMasterData
                                {
                                    ItemCode = item.ItemCode,
                                    ItemName = item.ItemName,
                                    ForeignName = item.ForeignName,
                                    AveragePrice = item.AveragePrice,
                                    SalesPrice = item.SalesPrice,
                                    BarCode = item.BarCode,
                                };

                                itemsOut.Add(itemc);

                                yield return new RequestResult<ItemMasterData>(Enums.StatusType.Success,
                                    $"Remaining ({items.Count - 1 - index} of {items.Count}).\r\n" +
                                    $"{message}  \r\n {requestBody.PrettyJson()} ",
                                    status, itemsOut, resultItemSync.Response);

                            }
                        }
                        else
                        {
                            var status = $"Cannot Add Item (Code: {item.ItemCode}, Name: {item.ItemName})";
                            var message = $"\r\nCannot Add Item.\r\n" +
                                          $"Request Body:\r\n" +
                                          $"{requestBody.PrettyJson()}\r\n\r\n" +
                                          $"Response Content:\r\n" +
                                          $"{resultItemSync.Response.Content.PrettyJson()} \r\n";

                            _loger.Error(message, "Error occurred.");

                            var requestResult = new RequestResult<ItemMasterData>(
                                Enums.StatusType.Failed, $"{remaining}.\r\n" +
                                                         $"{message}", status, itemsOut, resultItemSync.Response);

                            //_logger = new Logger<ItemMasterData>(Enums.StatusType.Failed, requestBody, message);

                            //_unitOfWork.ItemsLog.Add(_logger);
                            //_unitOfWork.SaveChanges();

                            yield return requestResult;
                        }
                    }
                }
            }
            else
            {
                var message = "There is no items available to be synced, or may it flagged as synced to prism or not active to be synced.";
                var Status = "Status: there is no items available to be synced, or may it flagged as synced to prism or not active to be synced.";

                _loger.Information(message);
                var requestResult = new RequestResult<ItemMasterData>(Enums.StatusType.Failed, message,
                    Status, itemsOut, new RestResponse());

                yield return requestResult;

            }

        }

        private static List<ItemMasterData> GetItems(string filter)
        {

            var query = Query() + filter;

            if (!ClientHandler.Company.Connected)
            {
                ClientHandler.InitializeClientObjects(_client, out _, out _);
            }

            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var items = new List<ItemMasterData>();

            oRecordSet.DoQuery(query);

            for (var i = 0; i < oRecordSet.RecordCount; i++)
            {
                var item = CreateItemMasterData(oRecordSet);
                items.Add(item);

                oRecordSet.MoveNext();
            }

            return items;
        }

        private static string Query()
        {
            var query = "SELECT DISTINCT" +
                        " T0.ItemCode 'ItemCode'" +
                        ",T0.ItemName 'ItemName'" +
                        ",T0.FrgnName 'ForeignName'" +
                        ",T0.U_Active 'Active'" +
                        ",T1.Code 'ColorCode'" +
                        ",T0.U_ColorGrpN 'ColorGroup'" +
                        ",T0.InvntryUom 'InventoryUoM'" +
                        ",T0.U_DesigGrpN 'DesigGroupName'" +
                        ",T0.U_Size 'Size'" +
                        ",T0.U_ProdGrpN 'ProductGroupName'" +
                        ",T0.CardCode 'CardCode'" +
                        ",T3.CardName 'CardName'" +
                        ",T4.ItmsGrpCod 'ItemGroupCode'" +
                        ",T4.ItmsGrpNam 'ItemGroupName'" +
                        ",T0.VATLiable 'IsTaxable'" +
                        ",T0.InvntItem 'InvntoryItem'" +
                        ",T0.AvgPrice 'AveragePrice'" +
                        ",T5.PriceList 'PriceListCode'" +
                        ",T5.Price 'SalesPrice'" +
                        ",T0.U_OrignGrpN 'OrignGroupName'" +
                        ",T0.U_Sticker 'Sticker'" +
                        ",T0.U_StickerF 'StickerForeign'" +
                        ",T0.U_TypeGrpN 'TypeGroupName'" +
                        ",T0.CodeBars 'BarCode'" +
                        ",T0.NumInSale 'ItemsPerSaleUoM'" +
                        ",T0.SalUnitMsr 'SalesUoM'" +
                        ",T0.[CreateDate] 'CreateDate'" +
                        ",T0.[UpdateDate] 'CreateDate'" +
                        "FROM " +
                        "   OITM T0 " +
                        "       LEFT JOIN [dbo].[@AICL]  T1  On T0.U_ColorGrpN = T1.Name " +
                        "       LEFT JOIN [dbo].[@CSIZ] T2 ON T0.U_Size = T2.Code " +
                        "       LEFT JOIN OCRD T3 ON T0.CardCode = T3.CardCode " +
                        "       LEFT JOIN OITB T4 ON T0.ItmsGrpCod = T4.ItmsGrpCod" +
                        "       LEFT JOIN ITM1 T5 ON T0.[ItemCode] = T5.[ItemCode]" +
                        "       LEFT JOIN OPLN T6 ON T5.PriceList = T6.ListNum" +
                        "           WHERE " +
                        "               T0.[U_Active] = 'Y' AND T5.PriceList = 1 ";
            return query;
        }

        private static ItemMasterData CreateItemMasterData(Recordset recordSet)
        {
            var item = new ItemMasterData();
            var field = recordSet.Fields;

            item.ItemCode = field.GetValue("ItemCode");
            item.ItemName = field.GetValue("ItemName");
            item.ForeignName = field.GetValue("ForeignName");
            item.Active = field.GetValue("Active").IsTrue();
            item.ColorCode = field.GetValue("ColorCode");
            item.ColorGroup = field.GetValue("ColorGroup");
            item.InventoryUoM = field.GetValue("InventoryUoM");
            item.OrignGroupName = field.GetValue("OrignGroupName");

            item.DesigGroupName = field.GetValue("DesigGroupName");

            //if (item.DesigGroupName.IsNullOrEmpty())
            //    item.DesigGroupName = item.ItemCode;

            item.Size = field.GetValue("Size");
            if (item.Size.IsNullOrEmpty())
                item.Size = item.ItemCode;

            item.ProductGroupName = field.GetValue("ProductGroupName");
            item.CardCode = field.GetValue("CardCode");
            item.CardName = field.GetValue("CardName");
            item.ItemGroupCode = field.GetValue("ItemGroupCode");
            item.ItemGroupName = field.GetValue("ItemGroupName");
            item.IsTaxable = field.GetValue("IsTaxable");

            if (field.GetValue("InvntoryItem") == "Y")
                item.InvntoryItem = false;
            else
                item.InvntoryItem = true;


            item.AveragePrice = Convert.ToDouble(field.GetValue("AveragePrice"));
            item.PriceListCode = Convert.ToInt32(field.GetValue("PriceListCode"));
            item.SalesPrice = field.GetValue("SalesPrice");
            item.Sticker = field.GetValue("Sticker");
            item.StickerForeign = field.GetValue("StickerForeign");
            item.TypeGroupName = field.GetValue("TypeGroupName");
            item.BarCode = field.GetValue("BarCode");
            item.ItemsPerSaleUoM = field.GetValue("ItemsPerSaleUoM");
            item.SalesUoM = field.GetValue("SalesUoM");

            return item;
        }

        public static void UpdateItem(string itemCode)
        {
            var query = $"Update OITM SET U_SyncToPrism = 'Y' WHERE ItemCode = '{itemCode}'";
            var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery(query);
        }
        public static async Task<bool> UpdateItem2(string itemCode)
        {
            var item = (Items)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oItems);

            if (item.GetByKey(itemCode)) // Retrieve the business partner by its item code
            {
                // Update the field value
                item.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";

                // Update the Item in the database
                int updateResult = item.Update();

                return updateResult == 0 ? // Update successful
                    Task.CompletedTask.IsCompletedSuccessfully
                    : Task.CompletedTask.IsFaulted;
            }
            return false;
        }
    }
}
