using SAPLink.Core;
using SAPLink.Core.Models;
using SAPLink.Core.Models.Prism.Inventory.Departments;
using SAPLink.Core.Models.SAP.MasterData.Items;
using SAPLink.Core.Models.System;
using SAPLink.EF;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Connection;
using SAPLink.Handler.Prism.Interfaces;
using SAPLink.Handler.SAP.Interfaces;

namespace SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;

public class DepartmentService : IEntityService<RequestResult<Department>, ItemGroups>
{
    private readonly Credentials? _credentials;
    private readonly Subsidiaries? _subsidiary;
    private readonly Clients? _client;

    public DepartmentService(Clients client)
    {
        _client = client;
        _credentials = client.Credentials.FirstOrDefault();
        _subsidiary = _credentials?.Subsidiaries.FirstOrDefault();
    }
   

    public async Task<RequestResult<Department>> GetByCodeAsync(string code)
    {
        RequestResult<Department> result = new();

        string query = _credentials.BackOfficeUri;
        //{{BackOffice}}/dcs?filter=(sbssid,eq,665151872000149257)AND(dcscode,eq,110)&cols=sid,dcscode,dname,cname,sname,dlongname,clongname,slongname,d,c,s,sbssid
        var resource = $"/dcs?filter=(sbssid,eq,{_subsidiary.SID})AND(dcscode,eq,{code})&cols=dcscode,dname,rowversion";

        result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);

        if (result.Response.StatusCode == HttpStatusCode.OK)
        {
            var entity = JsonConvert.DeserializeObject<Response<Department>>(result.Response.Content).Data.ToList()
                .FirstOrDefault(x => x.DcsCode == code);

            result.Status = Enums.StatusType.Success;
            result.EntityList.Add(entity);
        }
        return result;
    }


    public async Task<RequestResult<Department>> GetAll()
    {
        RequestResult<Department> result = new();

        try
        {
            var query = _credentials.BackOfficeUri;
            var resource =
                $"/dcs?filter=(sbssid,eq,{_subsidiary.SID})&cols=sid,d,c,s,dcscode,dname,cname,sname&sort=dname,asc;sid,asc";

            result.Response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);
            var content = result.Response.Content;

            if (result.Response.StatusCode == HttpStatusCode.OK)
            {
                var entites = JsonConvert.DeserializeObject<Response<Department>>(content).Data.ToList();
                result.EntityList.AddRange(entites);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"Can Not Get Product Prices - Exception Message: {e.Message}. \n\nResponse Error Message: {response.ErrorMessage}");
        }

        return result;
    }

    public async Task<RequestResult<Department>> AddAll(List<ItemGroups> list)
    {
        var body = CreateEntitiesPayload(list);
        return await Sync(body);
    }


    public async Task<RequestResult<Department>> Sync(string body, string sid = "", Enums.UpdateType updateType = Enums.UpdateType.InitialDepartment)
    {
        RequestResult<Department> result = new();

        try
        {
            var query = _credentials.BackOfficeUri;
            var resource = "/dcs";
            var method = Method.POST;

            if (updateType == Enums.UpdateType.SyncDepartment)
            {
                resource += $"/{sid}";
                method = Method.PUT;
            }
            result.Response = await HttpClientFactory.InitializeAsync(query, resource, method, body);

            return result;
        }
        catch (Exception e)
        {
            //
        }

        return result;
    }


    public async Task<string> CreateEntityPayload(ItemGroups item)
    {

        var productsList = new OdataPrism<Department>()
        {
            Data = new List<Department>()
            {
                new()
                {
                    OriginApplication = "RProPrismWeb",
                    SbsSid = _subsidiary.SID,
                    Active = true,
                    DepartmentName = item.ItemGroupName,
                    Regional = false,
                    DcsCode = item.ItemGroupCode,
                    Publishstatus = 2,
                    D = item.ItemGroupCode,
                }
            }
        };
        return JsonConvert.SerializeObject(productsList);
    }

    public async Task<string> CreateUpdatePayload(ItemGroups item, long rowVersion)
    {
        var productsList = new OdataPrism<DepartmentSyncRequest>()
        {
            Data = new List<DepartmentSyncRequest>()
            {
                new()
                {
                    Rowversion = rowVersion,
                    DepartmentName = item.ItemGroupName,
                    //Department = item.ItemGroupCode,
                }
            }
        };
        return JsonConvert.SerializeObject(productsList);
    }

    public string CreateEntitiesPayload(List<ItemGroups> itemGroupsList)
    {
        var dcsList = new List<Department>();

        foreach (var item in itemGroupsList)
        {
            dcsList.Add(new Department
            {
                OriginApplication = "RProPrismWeb",
                SbsSid = _subsidiary.SID,
                Active = true,
                DepartmentName = item.ItemGroupName,
                Regional = false,
                DcsCode = item.ItemGroupCode,
                Publishstatus = 2,
                D = item.ItemGroupCode,
                C = "",
                S = "",
            });
        }

        OdataPrism<Department> root = new OdataPrism<Department>
        {
            Data = dcsList,
        };

        return JsonConvert.SerializeObject(root);
    }

    #region Old

    public List<Department> AddDepartmentOld(Department categoryDto)
    {
        List<Department> dcs = null;
        try
        {
            var query = _credentials.BackOfficeUri;
            var resource = "/dcs";
            var body = @"
                                   {
                                    ""data"": [
                                           {
                                             ""originapplication"": ""RProPrismWeb"",
                                             ""sbssid"": """ + _subsidiary.SID + @""",
                                             ""dname"":""Test"", 
                                             ""regional"": false,
                                             ""d"":""" + categoryDto.D + @""",
                                             ""c"":""" + categoryDto.C + @""", 
                                             ""s"":""" + categoryDto.S + @""" 
                                            }
                                     ]
                                  }";


            var response = HttpClientFactory.InitializeAsync(query, resource, Method.POST, body).Result;

            if (response.Response.StatusCode == HttpStatusCode.OK)
            {
                JsonConvert.DeserializeObject<OdataPrism<Department>>(response.Response.Content).Data.ToList();
            }
            //else
            //    MessageBox.Show($"Can Not Find Product Prices - Response Status Code: {response.StatusCode}. \n\nResponse Content: {response.Content}");
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"Can Not Get Product Prices - Exception Message: {e.Message}. \n\nResponse Error Message: {response.ErrorMessage}");
        }

        return dcs;
    }

    #endregion

}