using System.Text;
using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Inventory.Products;
using SAPLink.Core.Models.System;
using SAPbobsCOM;
using SAPLink.Core.Utilities;
using SAPLink.EF;
using Department = SAPLink.Core.Models.Prism.Inventory.Departments.Department;
using ItemGroups = SAPLink.Core.Models.SAP.MasterData.Items.ItemGroups;
using SAPLink.Handler.Integration;
using SAPLink.Handler.SAP.Application;
using Serilog;
using Serilog.Core;
using ServiceLayerHelper.RefranceModels;
using System.Drawing;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;

public partial class DepartmentsHandler
{
    private readonly UnitOfWork _unitOfWork;
    private readonly DepartmentService _departmentService;
    private static Clients _client;
    private static ILogger _loger;
    public DepartmentsHandler(UnitOfWork unitOfWork, DepartmentService departmentService, Clients client)
    {
        _unitOfWork = unitOfWork;
        _departmentService = departmentService;
        _client = client;

        _loger = Helper.CreateLoggerConfiguration("Departments - (Item Groups)", "Handler", LogsTypes.InboundData);
    }
    public async IAsyncEnumerable<RequestResult<ItemGroups>> SyncAsync(string filter)
    {
        string logStatus = null;
        string logMessage = null;

        var outputGroupsList = new List<ItemGroups>();
        if (filter.IsNullOrEmpty())
            filter += " WHERE  (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";

        var itemGroups = GetItemsGroups(out logMessage, out logStatus, Enums.UpdateType.SyncDepartment, filter);

        if (itemGroups.Count > 0)
        {
            for (int i = 0; i < itemGroups.Count; i++)
            {
                var itemGroup = itemGroups[i];
                var requestBody = await _departmentService.CreateEntityPayload(itemGroup);

                var result = await _departmentService.Sync(requestBody);

                _loger.Information("-----------------------------------------------------------------");

                if (result.Response.StatusCode == HttpStatusCode.OK && result.Response.StatusCode == HttpStatusCode.OK)
                {
                    var categorizes = JsonConvert.DeserializeObject<Response<Department>>(result.Response.Content).Data.ToList();

                    if (categorizes.Any())
                    {
                        await UpdateItemGroup(itemGroup.ItemGroupCode);

                        logMessage += $"Item Group is Added ( {itemGroup.ItemGroupCode} : {itemGroup.ItemGroupName} ).\r\n";

                        _loger.Information($"Item Group is Added ( {itemGroup.ItemGroupCode} : {itemGroup.ItemGroupName} ).");
                        outputGroupsList.Add(itemGroup);

                        yield return new RequestResult<ItemGroups>(Enums.StatusType.Success, "", "", outputGroupsList, result.Response);

                        //SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialDepartment);
                        //SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncDepartment);
                    }
                }
                else if (result.Response.Content.Contains("Duplicate DCS found"))
                {
                    //statusLog = "Duplicates DCS found.";
                    //messageLog += $"Item Group is Duplicated ( {itemGroup.ItemGroupCode} : {itemGroup.ItemGroupName} ).\r\n";

                    var updateResult = await _departmentService.GetByCodeAsync(itemGroup.ItemGroupCode);
                    var department = updateResult.EntityList.FirstOrDefault();

                    if (updateResult.EntityList.Count == 0&&
                        updateResult.Status == Enums.StatusType.Failed || department == null)
                    {
                        logMessage = $"Cannot Add or Sync Department.\r\n" +
                                     $"Response Content:\r\n" +
                                     $"{updateResult.Response.Content}";

                        _loger.Error(logMessage, "Error occurred - ");

                        yield return new RequestResult<ItemGroups>(Enums.StatusType.Failed, logMessage,
                            "Cannot Add or Sync Department.", outputGroupsList, updateResult.Response);
                    }
                    else
                    {
                        var body = await _departmentService.CreateUpdatePayload(itemGroup, department.RowVersion);
                        var updateResult2 = await _departmentService.Sync(body, department.Sid, Enums.UpdateType.SyncDepartment);

                        if (updateResult2.Response.StatusCode == HttpStatusCode.OK)
                        {
                            var categorizes = JsonConvert
                                .DeserializeObject<Response<Department>>(updateResult2.Response.Content)
                                .Data.ToList();

                            if (categorizes.Any())
                            {
                                await UpdateItemGroup(itemGroup.ItemGroupCode);

                                logMessage += $"Item Group (Code: {itemGroup.ItemGroupCode}, Name: {itemGroup.ItemGroupName}) is updated and synced.\r\n";
                                _loger.Information($"Item Group (Code: {itemGroup.ItemGroupCode}, Name: {itemGroup.ItemGroupName}) is updated and synced.");
                                outputGroupsList.Add(itemGroup);

                                yield return new RequestResult<ItemGroups>(Enums.StatusType.Success, logMessage,
                                    $"Item Group (Code: {itemGroup.ItemGroupCode}, Name: {itemGroup.ItemGroupName}) is updated and synced.",
                                    outputGroupsList, result.Response);

                                //SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialDepartment);
                                //SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncDepartment);
                            }
                        }
                        else
                        {
                            logMessage = $"Cannot Add or Sync Department.\r\n" +
                                         $"Response Content:\r\n" +
                                         $"{updateResult2.Response.Content}";

                            _loger.Error(logMessage, "Error occurred - ");

                            yield return new RequestResult<ItemGroups>(Enums.StatusType.Failed, logMessage,
                                "Cannot Add or Sync Department.", outputGroupsList, updateResult2.Response);
                        }

                    }

                }
                else
                {
                    logMessage = $"Cannot Add or Sync Department.\r\n" +
                                  $"Response Content:\r\n" +
                                  $"{result.Response.Content}";

                    _loger.Error(logMessage, "Error occurred - ");

                    yield return new RequestResult<ItemGroups>(Enums.StatusType.Failed, logMessage,
                        "Cannot Add or Sync Department.", outputGroupsList, result.Response);
                }
            }
        }
        else
        {
            logMessage = "There is no Departments available to be synced or may it flagged as synced to prism.";
            logStatus = "Status: there is no Departments available to be synced or may it flagged as synced to prism.";

            _loger.Information(logMessage);
            yield return new RequestResult<ItemGroups>(Enums.StatusType.Failed, logMessage,
                logStatus, outputGroupsList, new RestResponse());
        }

    }

    private static void LogResponse(out string message)
    {
        string jsonString = "{\"name\":null,\"metatype\":null,\"comment\":null,\"translationid\":null,\"errors\":[{\"date\":\"2023-06-18T05:33:08.366+03:00\",\"class\":\"EPrismCoreException\",\"errorcode\":\"Department.4\",\"errormsg\":\"Duplicate DCS found\",\"httpcode\":400,\"httpmessage\":\"400 - Bad Request\",\"functionname\":\"Unknown\",\"paramvalues\":null}],\"data\":[]}";

        var myObject = JsonConvert.DeserializeObject<Response>(jsonString.Replace('/', ' '));

        StringBuilder sb = new StringBuilder();
        foreach (var error in myObject.Errors)
        {
            sb.AppendLine("Error:");
            //sb.AppendLine($"- Date: {error.Date}");
            //sb.AppendLine($"- Class: {error.Class}");
            sb.AppendLine($"- Error Code: {error.ErrorCode}");
            sb.AppendLine($"- Error Message: {error.ErrorMessage}");
            //sb.AppendLine($"- HTTP Status Code: {error.HttpStatusCode}");
            //sb.AppendLine($"- HTTP Message: {error.HttpMessage}");
            //sb.AppendLine($"- Function Name: {error.FunctionName}");
            //sb.AppendLine($"- Parameter Values: {error.ParamValues}");

            sb.AppendLine();
        }

        message = $"Cannot Add or Sync Department.\r\n" +
                  $"{sb}";
    }

    public static List<ItemGroups> GetItemsGroups(out string message, out string status,
        Enums.UpdateType type = Enums.UpdateType.InitialDepartment, string syncQuery = "")
    {
        var logMessage = "";

        var query = @"SELECT 
                            T0.[ItmsGrpCod], SUBSTRING(T0.[ItmsGrpNam], 1, 20) 'ItmsGrpNam', 
                            T0.[createDate], T0.[updateDate] 
                                   FROM OITB T0 ";

        if (type == Enums.UpdateType.SyncDepartment)
            query += syncQuery;

        if (ClientHandler.Company == null)
        {
            ClientHandler.InitializeClientObjects(_client, out var code, out var errorMessage);

            //logMessage = code != 0
            //    ? $"Error :{code}  : {errorMessage}"
            //    : "Not Connected to Database.";
        }

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        var itemGroups = new List<ItemGroups>();

        oRecordSet.DoQuery(query);

        for (var i = 0; i < oRecordSet.RecordCount; i++)
        {
            var item = new ItemGroups();
            item.ItemGroupCode = oRecordSet.Fields.Item("ItmsGrpCod").Value.ToString();
            item.ItemGroupName = oRecordSet.Fields.Item("ItmsGrpNam").Value.ToString();
            //item.CreateDate = oRecordSet.Fields.Item("createDate").Value.ToString();
            //item.UpdateDate = oRecordSet.Fields.Item("updateDate").Value.ToString();

            itemGroups.Add(item);

            oRecordSet.MoveNext();
        }
        message = logMessage;
        status = logMessage;
        return itemGroups;
    }
    public static async Task<bool> UpdateItemGroup(string itemGroupCode)
    {
        var itemGroup = (SAPbobsCOM.ItemGroups)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oItemGroups);

        if (itemGroup.GetByKey(Convert.ToInt32(itemGroupCode))) // Retrieve the Item Group by its card code
        {
            // Update the field value
            itemGroup.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";

            // Update the Item Group in the database
            int updateResult = itemGroup.Update();

            return updateResult == 0 ? // Update successful
                Task.CompletedTask.IsCompletedSuccessfully
                : Task.CompletedTask.IsFaulted;
        }
        return false;
    }
}