using SAPLink.Core.Models.SAP.MasterData.Items;
using SAPLink.Core.Models.System;
using SAPLink.Handler.Connected_Services;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory<SAPLink.Core.Models.Prism.Settings.PriceLevel>;

namespace SAPLink.Handler.Prism.Settings;

public class PriceLevelService
{
    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly Subsidiaries _subsidiary;

    public PriceLevelService(Clients client)
    {
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();
        _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
    }

    public async Task<IEnumerable<PriceLevel>?> GetPriceLevel(long subsidiarySid)
    {
        var query = $"/pricelevel?cols=rowversion,pricelvl,pricelvldescription,pricelvlname" +
                    $",active,discperc,secured,usediscperc,discperc,sbsno,sbssid,sid" +
                    $"&filter=(active,eq,1)AND(sbssid,eq,{subsidiarySid})&sort=pricelvlname,asc";

        var response = await HttpClientFactory.InitializeAsync(_credentials.CommonUri, query, Method.GET);
        var content = response.Response.Content;

        return response.Response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<OdataPrism<PriceLevel>>(response.Response.Content)?.Data.ToList()
            : null;
    }
    public async Task<List<PriceList>> VerifyPriceLevelExistence(List<PriceList> input, long subsidiarySid)
    {
        var output = new List<PriceList>();
        List<PriceLevel> priceLevelList;
        try
        {
            var query = _credentials.CommonUri;
            var resource = $"/pricelevel?cols=pricelvl,pricelvldescription,pricelvlname,active,sbssid,sid" +
                           $"&filter=(sbssid,eq,{subsidiarySid})AND(active,eq,1)&sort=pricelvlname,asc";

            var response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);
            var content = response.Response.Content;

            if (response.Response.StatusCode == HttpStatusCode.OK)
            {
                priceLevelList = JsonConvert.DeserializeObject<OdataPrism<PriceLevel>>(content).Data.ToList();

                var x = input.Where(a => priceLevelList.Any(x => x.Pricelvl != a.PriceListNo));

                output.AddRange(x);

                return output;
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"Can Not Get Product Prices - Exception Message: {e.Message}. \n\nResponse Error Message: {response.ErrorMessage}");
        }
        return output;
    }

    public bool AddPriceLevel(List<PriceList> input)
    {
        try
        {
            var query = _credentials.CommonUri;
            var resource = "/pricelevel";
            var body = CreatePriceLevelPayload(input);

            var response = HttpClientFactory.InitializeAsync(query, resource, Method.POST, body).Result;

            return response.Response.StatusCode == HttpStatusCode.OK;
            //JsonConvert.DeserializeObject<OdataPrism<CategoryDto>>(response.Response.Content).Data.ToList();
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"Can Not Get Product Prices - Exception Message: {e.Message}. \n\nResponse Error Message: {response.ErrorMessage}");
        }

        return false;
    }

    private string CreatePriceLevelPayload(List<PriceList> priceListsNotExistInPrism)
    {
        List<PriceLevel> priceLists = new List<PriceLevel>();

        foreach (var item in priceListsNotExistInPrism)
        {
            priceLists.Add(new PriceLevel
            {
                Originapplication = "RProPrismWeb",
                Active = true,
                Sbssid = _subsidiary.SID,
                Pricelvl = item.PriceListNo,
                Pricelvlname = item.PriceListName,
            });
        }
        var root = new OdataPrism<PriceLevel>
        {
            Data = priceLists,
        };

        return JsonConvert.SerializeObject(root);
    }

}