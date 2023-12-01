using SAPbobsCOM;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Sales;
using SAPLink.Handler.SAP.Application;
using Serilog;
using InventoryCounting = SAPLink.Core.Models.Prism.StockManagement.InventoryPosting;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.InventoryPosting;

public class InventoryPostingHandler
{
    private static ILogger _loger;

    static InventoryPostingHandler()
    {
        _loger = Helper.CreateLoggerConfiguration("Adjustments - (Inventory Posting)", "Handler", LogsTypes.OutboundData);
    }
    public async IAsyncEnumerable<RequestResult<InventoryCounting>> SyncInventoryPosting(List<InventoryCounting> inventoryCounting)
    {
        var result = new RequestResult<InventoryCounting>();

        if (inventoryCounting.Count > 0)
        {
            foreach (var count in inventoryCounting)
            {
                CompanyService oCS = ClientHandler.Company.GetCompanyService();
                InventoryPostingsService oInventoryPostingsService = (InventoryPostingsService)oCS.GetBusinessService(ServiceTypes.InventoryPostingsService);
                SAPbobsCOM.InventoryPosting oInventoryPosting = (SAPbobsCOM.InventoryPosting)oInventoryPostingsService.GetDataInterface(InventoryPostingsServiceDataInterfaces.ipsInventoryPosting);

                oInventoryPosting.CountDate = Convert.ToDateTime(count.CreateDate);
                oInventoryPosting.UserFields.Item("U_SyncToPrism").Value = "Y";
                oInventoryPosting.UserFields.Item("U_PrismSid").Value = $"{count.Sid}";
                foreach (var item in count.Adjitem)
                {
                    InventoryPostingLines oInventoryPostingLines = oInventoryPosting.InventoryPostingLines;
                    InventoryPostingLine oInventoryPostingLine = oInventoryPostingLines.Add();

                    decimal.TryParse(item.SalesPerUnitFactor, out var factor);

                    oInventoryPostingLine.ItemCode = item.Alu;
                    oInventoryPostingLine.WarehouseCode = count.StoreCode;
                    //oInventoryPostingLine.BinEntry = 2;
                    oInventoryPostingLine.Price = item.Price;
                    //oInventoryPostingLine.UoMCode = "Carton";
                    //oInventoryPostingLine.UoMCountedQuantity = 12;
                    //InventoryPostingBatchNumber oInventoryPostingBatchNumber = oInventoryPostingLine.InventoryPostingBatchNumbers.Add();
                    //oInventoryPostingBatchNumber.BatchNumber = "B-B1234";
                    //oInventoryPostingBatchNumber.Quantity = 

                    var qty = decimal.Parse(item.Adjvalue.ToString());
                    if (factor > 0)
                        oInventoryPostingLine.CountedQuantity = double.Parse((qty * factor).ToString());
                }
                oInventoryPosting.Remarks = $"Adjustment No. : {count.Adjno}\r\nAdjustment Sid : {count.Sid}";

                InventoryPostingParams oInventoryPostingParams = oInventoryPostingsService.Add(oInventoryPosting);

                // Check for errors
                if (oInventoryPostingParams.DocumentEntry > 0)
                {
                    var docEntry = oInventoryPostingParams.DocumentEntry;

                    result.EntityList.Add(count);
                    result.Message +=
                        $"\r\nInventory Posting ({docEntry}) created successfully!.\r\n" +
                        $"\r\nSuccessfully Update Sync Flag for Adjustment No.: {count.Adjno} - Adjustment Sid : {count.Sid}.";

                    _loger.Information(result.Message);

                    result.Status = Enums.StatusType.Success;

                    yield return result;
                }
                else
                {
                    ClientHandler.Company.GetLastError(out var errorCode, out var errorMessage);

                    result.Message += $"\r\nFailed to create Inventory Posting. \r\nError {result.Message}: {errorMessage}";
                    result.Status = Enums.StatusType.Failed;

                    _loger.Information(result.Message);

                    yield return result;
                }
            }
        }
        else
        {
            result.Message = "There is no Inventory Posting available to be synced or may it flagged as synced to SAP.";
            result.StatusBarMessage = $"Status: {result.Message}";
            result.Status = Enums.StatusType.NotFound;

            _loger.Information(result.Message);

            yield return result;
        }
    }
}