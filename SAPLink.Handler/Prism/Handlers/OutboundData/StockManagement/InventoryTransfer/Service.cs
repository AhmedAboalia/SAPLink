using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Receiving;
using SAPLink.Core.Models.Prism.Sales;
using SAPLink.Core.Models.Prism.StockManagement;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.Handler.Prism.Settings;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory;
using PrismInvoice = SAPLink.Core.Models.Prism.Sales.Invoice;

namespace SAPLink.Handler.Prism.Handlers.OutboundData.StockManagement.VerifiedVouchers;

public partial class Service
{
    private readonly Credentials? _credentials;
    private readonly Subsidiaries? _subsidiary;
    public readonly TaxCodesService? TaxCodesService;
    public List<TaxCodes>? TaxCodes;
    private static ILogger _loger;

    public StoresService? StoresService;
    public List<Store>? Stores;

    static Service()
    {
        _loger =  Helper.CreateLoggerConfiguration("Verified Vouchers", "Service", LogsTypes.OutboundData);
    }

    public Service(Clients client)
    {
        _credentials = client.Credentials.FirstOrDefault();
        _subsidiary = _credentials?.Subsidiaries.FirstOrDefault();
        TaxCodesService = new TaxCodesService(client);
        StoresService = new StoresService(client);
    }
   
    public async Task<RequestResult<VerifiedVoucher>> GetVerifiedVoucher(DateTime dateFrom, DateTime dateTo, string vouchersNo = "")
    {
        RequestResult<VerifiedVoucher> result = new();
        try
        {
            string query = _credentials.BackOfficeUri;


            var from = dateFrom.ToPrismFromDateFormat();
            var to = dateTo.ToPrismToDateFormat();
            string dateRange = $"AND(createddatetime,ge,{from})AND(postdate,le,{to})"; //AND(postdate,le,{to})";  //(createddatetime,ge,2023-07-31T21:00:00.000Z)


            string vouchersFilter = "";
            if (vouchersNo.IsHasValue())
                vouchersFilter = $"AND(vouno,eq,{vouchersNo})";

            var resource = $"/receiving" +
                           $"?filter=(sbssid,eq,{_subsidiary.SID}){dateRange}AND(status,eq,4)AND(vouclass,ne,2)AND(slipflag,eq,1)AND(verified,eq,true){vouchersFilter}" + ///AND(Trackingno,ne,)
                           $"&cols=slipsbsno,vouno,storesid,origstoresid,slipstorecode,rowversion,storeno,storename,storecode,origstorecode,origstoreno,origstorename,recvitem.qty,recvitem.itemsid,recvitem.itemsid,recvitem.description1,recvitem.description2,recvitem.alu,recvitem.price,recvitem.upc,pkgno,slipno";

            result.Message = $"Resource: \r\n" +
                             $"{query}{resource}\r\n" +
                             $"Auth Session: {_credentials.AuthSession}";

            result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);


            if (result.Response.StatusCode == HttpStatusCode.OK)
            {
                var invoices = VerifiedVoucher.FromJson(result.Response.Content).Data;
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

    private RequestResult<VerifiedVoucher> LoadMockData(string fileName)
    {
        var file = File.ReadAllText($"Resources\\{fileName}");
        var invoices = VerifiedVoucher.FromJson(file).Data;

        RequestResult<VerifiedVoucher> result = new RequestResult<VerifiedVoucher>();

        result.EntityList.AddRange(invoices);
        return result;
    }

    public async Task<RequestResult<VerifiedVoucher>> UpdateIsSynced(string receivingSid, string rowVersion, string trackingNo, string note, string storeCode)
    {
        RequestResult<VerifiedVoucher> result = new();

        var query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receivingSid}";

        var store = await GetStore(storeCode);
        var storeSid = store.Sid;

        var body = ReceivingRequest.CreateBody(_subsidiary.Clerksid, rowVersion, trackingNo, note, storeSid);
        result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body);


        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            result.Status = Enums.StatusType.Success;
            result.Message = $"Successfully Update Sync Flag for the Verified Voucher SID: {receivingSid}.";
            _loger.Information(result.Message);
        }
        else
        {
            result.Status = Enums.StatusType.Failed;
            result.Message = $"Failed to Update Sync Flag for the Prism invoice SID: {receivingSid}" +
                             $"\r\nRequest EndPoint: {query}{resource}" +
                             $"\r\nBody:\r\n" +
                             $"{body.PrettyJson()}\r\n" +
                             $"\r\nStatus Code: {result.Response.StatusCode}" +
                             $"\r\nContent: {result.Response.Content.PrettyJson()}\r\n";
            
            _loger.Error(result.Message);
        }

        return result;
    }

    public async Task<Store> GetStore(string storeCode)
    {
        string query = $"/v1/rest/store?filter=(active,eq,true)AND(subsidiary_sid,eq,{_subsidiary.SID})AND(store_code,eq,{storeCode})&cols=sid,store_code,store_name,active&sort=store_code,asc";

        var response = await HttpClientFactory.InitializeAsync(_credentials.BaseUri, query, Method.GET);
        var content = response.Content;

        return response.StatusCode == HttpStatusCode.OK
            ? Store.FromJson(content)
                .FirstOrDefault(x => x.StoreCode == storeCode)
            : null;
    }
}