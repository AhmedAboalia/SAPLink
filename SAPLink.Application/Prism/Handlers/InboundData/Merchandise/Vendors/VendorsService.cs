using SAPLink.Application.Connection;
using SAPLink.Application.Prism.Interfaces;
using SAPLink.Domain;
using SAPLink.Domain.Models;
using SAPLink.Domain.Models.Prism.Merchandise.Vendor;
using SAPLink.Domain.Models.SAP.MasterData.BusinessPartners;
using SAPLink.Domain.Models.System;

namespace SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Vendors;

public class VendorsService : IEntityService<RequestResult<Vendor>, BusinessPartner>
{
    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly Subsidiaries _subsidiary;

    public VendorsService(Clients client)
    {
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();
        _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
    }
    public async Task<RequestResult<Vendor>> GetByCodeAsync(string cardCode)
    {
        RequestResult<Vendor> result = new();

        var query = _credentials.BackOfficeUri;
        var resource = $"/vendor?filter=(sbssid,eq,{_subsidiary.SID})AND(vendcode,eq,{cardCode})";

        result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);

        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            var entity = JsonConvert.DeserializeObject<Response<Vendor>>(result.Response.Content).Data.ToList()
                .FirstOrDefault();

            result.EntityList.Add(entity);
        }
        return result;
    }

    public async Task<RequestResult<Vendor>> AddAsync(List<BusinessPartner> businessPartnerList)
    {
        var body = CreateEntitiesPayload(businessPartnerList);
        return await Sync(body);
    }

    public async Task<RequestResult<Vendor>> AddAsync(string body)
    {
        return await Sync(body);
    }

    public async Task<RequestResult<Vendor>> Sync(string body, string sid = "", Enums.UpdateType updateType = Enums.UpdateType.InitialVendors)
    {
        RequestResult<Vendor> result = new();

        var query = _credentials.BackOfficeUri;
        var resource = "/vendor";
        var method = Method.POST;

        if (updateType == Enums.UpdateType.SyncVendors)
        {
            resource += $"/{sid}";
            method = Method.PUT;
        }

        result.Response = await HttpClientFactory.InitializeAsync(query, resource, method, body);

        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            var entity = JsonConvert.DeserializeObject<Response<Vendor>>(result.Response.Content).Data.ToList()
                .FirstOrDefault();

            result.EntityList.Add(entity);
        }
        return result;
    }


    public string CreateEntitiesPayload(List<BusinessPartner> businessPartnersList)
    {
        var vendors = new List<Vendor>();

        foreach (var item in businessPartnersList)
        {
            vendors.Add(new Vendor
            {
                Originapplication = "RProPrismWeb",
                Vendcode = item.CardCode,
                Vendname = item.CardName,
                Sbssid = _subsidiary.SID.ToString(),
                Active = true,
            });
        }
        var root = new OdataPrism<Vendor>
        {
            Data = vendors,
        };

        return JsonConvert.SerializeObject(root);
    }

    public async Task<string> CreateEntityPayload(BusinessPartner businessPartner)
    {
        var productsList = new OdataPrism<Vendor>()
        {
            Data = new List<Vendor>()
            {
                new()
                {
                    Originapplication = "RProPrismWeb",
                    Vendcode = businessPartner.CardCode,
                    Vendname = businessPartner.CardName,
                    Sbssid = _subsidiary.SID.ToString(),
                    Active = true,
                }
            }
        };
        return JsonConvert.SerializeObject(productsList);
    }

    public async Task<string> CreateUpdatePayload(BusinessPartner businessPartner, long rowVersion)
    {
        var productsList = new OdataPrism<VendorSyncRequest>()
        {
            Data = new List<VendorSyncRequest>()
            {
                new()
                {
                    Rowversion = rowVersion,
                    Vendname = businessPartner.CardName,
                    Sbssid = _subsidiary.SID.ToString(),
                }
            }
        };
        return JsonConvert.SerializeObject(productsList);
    }
}