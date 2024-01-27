﻿using SAPLink.Core.Models.System;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Connection;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory<SAPLink.Core.Models.Prism.Settings.Store>;

namespace SAPLink.Handler.Prism.Settings
{
    public class StoresService
    {
        private readonly Clients _client;
        private readonly Credentials _credentials;
        private readonly Subsidiaries _subsidiary;

        public StoresService(Clients client)
        {
            _client = client;
            _credentials = client.Credentials.FirstOrDefault();
            _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
        }

        public async Task<List<Store>> GetAll()
        {
            var query = _credentials.BaseUri;
            var resource = $"/v1/rest/store" +
                           $"?cols=sid,store_name,store_number,store_code,active,subsidiary_sid,active_price_level_sid" +
                           $"&filter=(active,eq,true)AND(subsidiary_sid,eq,{_subsidiary.SID})&sort=store_number,asc";

            var response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);

            if (response.Response.StatusCode == HttpStatusCode.OK)
            {

                return Store.FromJson(response.Response.Content).ToList();
            }
            return new List<Store>();

            //return LoadMockData();
        }

        private List<Store> LoadMockData()
        {
            var file = File.ReadAllText(@"Resources\TestStores.json");
            return Store.FromJson(file).ToList();
        }
    }
}
