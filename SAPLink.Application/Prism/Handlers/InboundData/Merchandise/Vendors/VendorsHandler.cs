using System.Text;
using SAPbobsCOM;
using SAPLink.Application.HangFire;
using SAPLink.Application.SAP.Application;
using SAPLink.Domain;
using SAPLink.Domain.Common;
using SAPLink.Domain.Models.Prism.Merchandise.Vendor;
using SAPLink.Domain.System;
using SAPLink.Domain.Utilities;
using SAPLink.Handler.Prism.Handlers;
using SAPLink.Infrastructure;
using Serilog;
using BusinessPartner = SAPLink.Domain.SAP.MasterData.BusinessPartners.BusinessPartner;

namespace SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Vendors;

public partial class VendorsHandler
{
    private Clients _client;
    private static VendorsService _vendorsService;
    private static UnitOfWork _unitOfWork;
    private static ILogger _loger;

    public VendorsHandler(UnitOfWork unitOfWork, Clients client)
    {
        _client = client;
        _unitOfWork = unitOfWork;
        _vendorsService = new VendorsService(client);

        _loger = Helper.CreateLoggerConfiguration("Vendors - (Business Partners)", "Handler", LogsTypes.InboundData);
    }
    public async IAsyncEnumerable<RequestResult<BusinessPartner>> SyncAsync(string filter)
    {
        string logStatus = null;
        string logMessage = null;

        var outbusinessPartners = new List<BusinessPartner>();
        var businessPartners = new List<BusinessPartner>();

        if (filter.IsNullOrEmpty())
            businessPartners = GetVendors();
        else
            businessPartners = GetVendors(Enums.UpdateType.SyncVendors, filter);

        if (businessPartners.Count > 0)
        {
            for (int i = 0; i < businessPartners.Count; i++)
            {
                var businessPartner = businessPartners[i];
                var requestBody = await _vendorsService.CreateEntityPayload(businessPartner);

                var result = await _vendorsService.AddAsync(requestBody);
                _loger.Information("-----------------------------------------------------------------");

                if (result.Response.StatusCode == HttpStatusCode.OK)
                {
                    var vendorsList = JsonConvert.DeserializeObject<OdataPrism<VendorSyncRequest>>(result.Response.Content).Data.ToList();

                    if (vendorsList.Any())
                    {
                        logMessage += $"Vendor is Added (Code {businessPartner.CardCode} : Name {businessPartner.CardName} ).\r\n";

                        SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialVendors);
                        SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncVendors);

                        _loger.Information($"Vendor is Added (Code {businessPartner.CardCode} : Name {businessPartner.CardName} ).");

                        outbusinessPartners.Add(businessPartner);
                        yield return new RequestResult<BusinessPartner>(Enums.StatusType.Success, "", "", outbusinessPartners, result.Response);
                    }
                }
                else if (result.Response.Content.Contains("User defined vendor code already exists"))
                {

                    var result2 = await _vendorsService.GetByCodeAsync(businessPartner.CardCode);

                    var vendorPrism = result2.EntityList.FirstOrDefault();
                    var body = await _vendorsService.CreateUpdatePayload(businessPartner, vendorPrism.Rowversion);
                    var updaterResult = await _vendorsService.Sync(body, vendorPrism.Sid, Enums.UpdateType.SyncVendors);

                    if (updaterResult.Response.StatusCode == HttpStatusCode.OK)
                    {
                        var vendors = JsonConvert.DeserializeObject<OdataPrism<VendorSyncRequest>>(updaterResult.Response.Content).Data.ToList();

                        if (vendors.Any())
                        {
                            logMessage += $"Vendor is updated and synced (Code: {businessPartner.CardCode}, Name: {businessPartner.CardName}).\r\n";
                            await UpdateBusinessPartner(businessPartner.CardCode);

                            outbusinessPartners.Add(businessPartner);
                            yield return new RequestResult<BusinessPartner>(Enums.StatusType.Success, "", "", outbusinessPartners, updaterResult.Response);
                            
                            _loger.Information($"Vendor is updated and synced (Code: {businessPartner.CardCode}, Name: {businessPartner.CardName}).");
                            SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.InitialVendors);
                            SyncEntityService.UpdateSyncEntityDate(_unitOfWork, Enums.UpdateType.SyncVendors);
                        }
                    }
                    else
                    {
                        logMessage = $"Cannot Add or Sync Vendor.\r\n" +
                                     $"Response Content:\r\n" +
                                     $"{updaterResult.Response.Content}";

                        _loger.Error(logMessage, "Error occurred.");
                        yield return new RequestResult<BusinessPartner>(Enums.StatusType.Failed, logMessage,
                            "An error occurred during vendor synchronize.", outbusinessPartners, result2.Response);

                    }
                }
                else
                {
                    logMessage = $"Cannot Add or Sync Vendor.\r\n" +
                                 $"Response Content:\r\n" +
                                 $"{result.Response.Content}";
                    _loger.Information(logMessage);
                    yield return new RequestResult<BusinessPartner>(Enums.StatusType.Failed, logMessage,
                        "An error occurred during vendor synchronize.", outbusinessPartners, result.Response);
                }
            }
        }
        else
        {
            logMessage = "There is no vendors available to be synced or may it flagged as synced to prism.";
            logStatus = "Status: there is no vendors available to be synced or may it flagged as synced to prism.";

            _loger.Information(logMessage);
            yield return new RequestResult<BusinessPartner>(Enums.StatusType.Success, logMessage, logStatus, null, null);
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

        message = $"Cannot Add or Sync Vendor \r\n \r\n {sb}";
    }

    public static List<BusinessPartner> GetVendors(Enums.UpdateType type = Enums.UpdateType.InitialVendors,
        string syncQuery = "")
    {
        var query = @"SELECT 
                        DISTINCT 
                            SUBSTRING(T0.CardCode, 1, 6) 'CardCode' , 
                            SUBSTRING(T0.CardName, 1, 25) 'CardName', 
                            T0.[CreateDate], T0.[UpdateDate] 
                                FROM OCRD T0 
                                    WHERE T0.CardType = 'S' ";
                                         //AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";

        if (type == Enums.UpdateType.SyncVendors)
            query += syncQuery;

        var oRecordSet = (Recordset)ClientHandler.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        var businessPartners = new List<BusinessPartner>();

        oRecordSet.DoQuery(query);

        for (var i = 0; i < oRecordSet.RecordCount; i++)
        {
            var businessPartner = new BusinessPartner();
            businessPartner.CardCode = oRecordSet.Fields.Item("CardCode").Value.ToString();
            businessPartner.CardName = oRecordSet.Fields.Item("CardName").Value.ToString();
            //businessPartner.CreateDate = oRecordSet.Fields.Item("CreateDate").Value.ToString();
            //businessPartner.UpdateDate = oRecordSet.Fields.Item("UpdateDate").Value.ToString();

            businessPartners.Add(businessPartner);

            oRecordSet.MoveNext();
        }

        return businessPartners;
    }

    public class SyncResult<T>
    {
        public string Message { get; set; }
        public string StatusBarMessage { get; set; }
        public Enums.StatusType Status { get; set; }
        public List<T> EntityList { get; set; }
        public IRestResponse Response { get; set; }



        public SyncResult(Enums.StatusType status, string message, string statusBarMessage, List<T> entityList, IRestResponse response)
        {
            Status = status;
            Message = message;
            StatusBarMessage = statusBarMessage;
            EntityList = entityList;
            Response = response;
        }
    }

    public static async Task<bool> UpdateBusinessPartner(string cardCode)
    {
        var businessPartners = (BusinessPartners)ClientHandler.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

        if (businessPartners.GetByKey(cardCode)) // Retrieve the business partner by its card code
        {
            // Update the field value
            businessPartners.UserFields.Fields.Item("U_SyncToPrism").Value = "Y";

            // Update the business partner in the database
            int updateResult = businessPartners.Update();

            return updateResult == 0 ? // Update successful
                Task.CompletedTask.IsCompletedSuccessfully
                : Task.CompletedTask.IsFaulted;
        }
        return false;
    }
}