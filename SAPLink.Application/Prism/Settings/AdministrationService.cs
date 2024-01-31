using SAPLink.Application.Connected_Services;
using SAPLink.Application.Connection;
using SAPLink.Domain.Models.Prism.Settings;
using SAPLink.Domain.System;

namespace SAPLink.Application.Prism.Settings;

public class AdministrationService
{
    private readonly Clients _client;
    private readonly Credentials _credentials;
    private readonly Subsidiaries _subsidiary;
    public AdministrationService(Clients client)
    {
        _client = client;
        _credentials = _client.Credentials.FirstOrDefault();
        _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
    }

    public async Task<List<Subsidiary>?> GetSubsidiaries()
    {
        var query = "/v1/rest/subsidiary?cols=sid,subsidiary_number,subsidiary_name,active_price_level_sid,active_season_sid,price_level_name&sort=subsidiary_name,asc";

        var response = await HttpClientFactory.InitializeAsync(_credentials.BaseUri, query, Method.GET);
        var content = response.Content;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<List<Subsidiary>>(content)
            : null;
    }
    public async Task<Subsidiary?> GetSubsidiaryByNumber(string text)
    {
        var query = $"/v1/rest/subsidiary?filter=(subsidiary_number,eq,{text})&sort=subsidiary_name,asc&cols=sid,subsidiary_number,subsidiary_name,active_price_level_sid,active_season_sid,price_level_name";

        var response = await HttpClientFactory.InitializeAsync(_credentials.BaseUri, query, Method.GET);
        var content = response.Content;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<Subsidiary>(content)
            : null;
    }
    public IEnumerable<Subsidiary>? GetSubsidiaryBySid()
    {
        var query = $"/v1/rest/subsidiary?filter=(sid,eq,{_subsidiary.SID})&sort=subsidiary_name,asc&cols=sid,subsidiary_number,subsidiary_name,active_price_level_sid,active_season_sid,price_level_name";

        var response = HttpClientFactory.InitializeAsync(_credentials.BaseUri, query, Method.GET).Result;
        var content = response.Content;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<List<Subsidiary>>(content)
            : null;
    }
    public Season GetSeason(long seasonSid)
    {
        const string query = "/api/common/season?cols=*&filter=(active,eq,true)&sort=seasonname,asc";

        var response = HttpClientFactory.InitializeAsync(_credentials.BaseUri, query, Method.GET).Result;
        var content = response.Content;

        return response.StatusCode == HttpStatusCode.OK
            ? JsonConvert.DeserializeObject<OdataPrism<Season>>(content).Data.ToList()
                .FirstOrDefault(x => x.SeasonId == seasonSid)
            : null;
    }
}