using SAPLink.Application.Connection;
using SAPLink.Application.Prism.Settings;
using SAPLink.Domain.Common;
using SAPLink.Domain.Models.Prism.Settings;
using SAPLink.Domain.System;
using SAPLink.Domain.Utilities;
using SAPLink.Handler.Prism.Handlers;
using Serilog;
using InventoryPostings = SAPLink.Domain.Models.Prism.StockManagement.InventoryPosting;

namespace SAPLink.Application.Prism.Handlers.OutboundData.StockManagement.InventoryPosting;

public partial class InventoryPostingService
{
    private readonly Credentials? _credentials;
    private readonly Subsidiaries? _subsidiary;
    public readonly TaxCodesService? TaxCodesService;
    public List<TaxCodes>? TaxCodes;

    public StoresService? StoresService;
    public List<Store>? Stores;
    private static ILogger _loger;

    static InventoryPostingService()
    {
        _loger = Helper.CreateLoggerConfiguration("Inventory Posting", "Service", LogsTypes.OutboundData);
    }

    public InventoryPostingService(Clients client)
    {
        _credentials = client.Credentials.FirstOrDefault();
        _subsidiary = _credentials?.Subsidiaries.FirstOrDefault();
        TaxCodesService = new TaxCodesService(client);
        StoresService = new StoresService(client);
    }

    public async Task<RequestResult<InventoryPostings>> GetInventoryPosting(int storeNumber, string filter, string DocCode = "")
    {
        var result = new RequestResult<InventoryPostings>();
        try
        {
            string query = _credentials.BackOfficeUri;


            //var to = dateTo.ToPrismToDateFormat();
            string storeCodeFilter = "";
            Stores = await StoresService.GetAll();
            if (storeNumber != -1)
            {
                var storeCode = Stores.FirstOrDefault(s => s.StoreNumber == storeNumber).StoreCode;
                if (storeCode.IsHasValue())
                    storeCodeFilter = $"AND(storecode,eq,{storeCode})";
            }

            string DocCodeFilter = "";
            if (DocCode.IsHasValue())
                DocCodeFilter = $"AND(adjno,eq,{DocCode})";

            //
            var resource = $"/adjustment" +
                           $"?filter=(sbssid,eq,{_subsidiary.SID}){storeCodeFilter}{DocCodeFilter}{filter}" +
                           $"&cols=sid,rowversion,adjno,adjtype,status,verified,reasonname,storecode,creatingdoctype,adjitem.sid,adjitem.rowversion,adjitem.itemsid,adjitem.alu,adjitem.origvalue,adjitem.adjvalue,adjitem.description1,adjitem.description2,adjitem.price,adjitem.cost,adjitem.size";

            //api/backoffice/adjustment
            //?filter=(creatingdoctype,eq,1)AND(status,eq,4)
            //AND(storecode,eq,001)AND(adjtype,eq,0)AND(sbssid,eq,675951888000146257)
            //&count=true&page_no=1&page_size=30&cols=sid,rowversion,adjno,adjtype,status,verified,storecode,adjitem.sid,adjitem.rowversion,adjitem.itemsid,adjitem.alu,adjitem.origvalue,adjitem.adjvalue,adjitem.description1,adjitem.description2

            result.Message = $"Resource: \r\n" +
                             $"{query}{resource}\r\n" +
                             $"Auth Session: {_credentials.AuthSession}";

            result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);


            if (result.Response.StatusCode == HttpStatusCode.OK)
            {
                var invoices = InventoryPostings.FromJson(result.Response.Content).Data;
                result.EntityList.AddRange(invoices);
                _loger.Information("Successfully fetched the Verified Voucher/s.");
            }
            else
                _loger.Warning($"Failed to fetch Verified Voucher/s. Status Code: {result.Response.StatusCode}. Content: {result.Response.Content}");

            return result;

            //return LoadMockData("Order.json");
        }
        catch (Exception ex)
        {
            _loger.Error(ex, "Error occurred while fetching the Verified Voucher/s.");
            //throw;  // re-throwing the exception if you want the caller to handle it, or you can handle it here as well.
        }
        finally
        {
            //Log.CloseAndFlush();
        }

        return result;
    }

    private RequestResult<InventoryPostings> LoadMockData(string fileName)
    {
        var file = File.ReadAllText($"Resources\\{fileName}");
        var invoices = InventoryPostings.FromJson(file).Data;

        RequestResult<InventoryPostings> result = new RequestResult<InventoryPostings>();

        result.EntityList.AddRange(invoices);
        return result;
    }


    public async Task<bool> UpdateIsSynced(string invoiceSid, string rowVersion, string InvoiceNo = "")
    {
        string query = _credentials.BaseUri;
        var resource = $"/v1/rest/document/{invoiceSid}/?filter=row_version,eq,{rowVersion}";

        string body = @"[
                              {
                                  ""pos_flag3"": ""Yes"",
                                  ""comment2"": """ + InvoiceNo + @"""
                              }
                          ]";

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body);

        return response.StatusCode == HttpStatusCode.OK;
    }
}