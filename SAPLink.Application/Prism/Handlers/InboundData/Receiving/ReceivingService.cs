using SAPLink.Application.Connected_Services;
using SAPLink.Application.Connection;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Application.Prism.Interfaces;
using SAPLink.Domain.Models.Prism.Merchandise.Inventory;
using SAPLink.Domain.Models.Prism.Receiving;
using SAPLink.Domain.Models.Prism.Settings;
using SAPLink.Domain.Models.SAP.Documents;
using SAPLink.Domain.Models.System;

namespace SAPLink.Application.Prism.Handlers.InboundData.Receiving;

public class ReceivingService : IReceivingService
{
    private static ItemsService _itemsService;
    private static DepartmentService _departmentServicess;
    private static VendorsService _vendorsService;

    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly Subsidiaries _subsidiary;

    public ReceivingService(Clients client)
    {
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();
        _subsidiary = _credentials.Subsidiaries.FirstOrDefault();

        _departmentServicess = new DepartmentService(_client);
        _vendorsService = new VendorsService(_client);

        _itemsService = new ItemsService(_client, _departmentServicess, _vendorsService);
    }

    public async Task<ReceivingResponseDto> GenerateVoucherSid(string storeSid)
    {
        string query = _credentials.BackOfficeUri;
        var resource = "/receiving";
        var body = ReceivingResponse.CreateBody(
            _subsidiary.SID.ToString(),
            _subsidiary.Clerksid,
            storeSid);

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.POST, body);

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<OdataPrism<ReceivingResponseDto>>(response.Content).Data.ToList().FirstOrDefault()
            : new ReceivingResponseDto();
    }

    public async Task<IRestResponse> AddConsolidateItem(string body, string receivingSid)
    {
        string query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receivingSid}?action=AddConsolidateVouItem";

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.POST, body);

        return response;
        //return response.StatusCode == HttpStatusCode.OK
        //    ? JsonConvert.DeserializeObject<ReceivingItemData>(response.Content)
        //    : new ReceivingItemData();
    }

    public Comment AddGrpoComment(string comment, string receivingSid)
    {
        string query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receivingSid}/recvcomment";

        string body = @"{
                          ""data"": [
                              {
                                  ""originapplication"": ""RProPrismWeb"",
                                  ""comments"": """ + comment + @""",
                                  ""vousid"": """ + receivingSid + @"""
                              }
                          ]
                          }";

        var response = HttpClientFactory.InitializeAsync(query, resource, Method.POST, body).Result;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<OdataPrism<Comment>>(response.Content).Data.FirstOrDefault()
            : new Comment();
    }
    public async Task<bool> ChangeLocation(string storeSid)
    {
        string query = _credentials.BaseUri;
        var resource = "/v1/rpc";

        string body = @"[
                           {
                          ""MethodName"": ""ChangeSubStoreMethod"",
                          ""Params"":
                              {
                                  ""SubsidiarySid"": """ + _subsidiary.SID + @""",
                                  ""StoreSid"": """ + storeSid + @"""
                              }
                          }
                            ]";

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.POST, body);

        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> ChangeReceivingToReturn(string receivingSid, string rowVersion)
    {
        string query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receivingSid}";

        string body = @"{""data"":[{""rowversion"":" + rowVersion + @",""voutype"":1}]}";

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body);

        return response.StatusCode == HttpStatusCode.OK;
    }
    public Comment AddGrpoTrackingNumAndNote(string GrpoNo, string receivingSid, string rowVersion, string note)
    {
        string query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receivingSid}";

        string body = @"{
                          ""data"": [
                              {
                                  ""rowversion"": """ + GrpoNo + @""",
                                  ""trackingno"": """ + rowVersion + @""",
                                  ""note"": """ + note + @"""
                              }
                          ]
                          }";

        var response = HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body).Result;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<OdataPrism<Comment>>(response.Content).Data.FirstOrDefault()
            : new Comment();
    }

    public string CreateAddConsolidateItemPayload(ProductResponseModel product, string sid, Line line)
    {
        return ReceivingItemRequest.CreateBody(product, sid, line);
    }

    public async Task<ReceivingResponseDto> GetReceiving(ReceivingResponseDto receiving)
    {
        var query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receiving.Sid}";

        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET, "");
        return JsonConvert.DeserializeObject<OdataPrism<ReceivingResponseDto>>(response.Content).Data.ToList().FirstOrDefault();

    }
    public async Task<IRestResponse> AddReceiving(ReceivingResponseDto receiving, string rowVersion, string trackingNo, string note, string storeSid)
    {
        var query = _credentials.BackOfficeUri;
        var resource = $"/receiving/{receiving.Sid}";

        var body = ReceivingRequest.CreateBody(_subsidiary.Clerksid, rowVersion, trackingNo, note, storeSid);
        var response = await HttpClientFactory.InitializeAsync(query, resource, Method.PUT, body);

        return response;
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