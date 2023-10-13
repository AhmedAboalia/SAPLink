using SAPLink.Core.Models.System;
using SAPLink.Handler.Connected_Services;
using SAPLink.Handler.Connection;

namespace SAPLink.Handler.Prism.Settings
{
    public class TaxCodesService
    {
        private readonly Credentials _credentials;
        private readonly Subsidiaries _subsidiary;
        public TaxCodesService(Clients client)
        {
            _credentials = client.Credentials.FirstOrDefault();
            _subsidiary = _credentials.Subsidiaries.FirstOrDefault();
        }

        public async Task<List<TaxCodes>> GetAll()
        {
            var query = _credentials.BaseUri;
            var resource = $"/api/common/taxcode" +
                           $"?cols=sid,sbssid,taxcode,taxname,isdefault" +
                           $"&filter=(sbssid,eq,{_subsidiary.SID})&sort=taxcode,asc";

            var response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);


            if (response.StatusCode == HttpStatusCode.OK)
                return TaxCodes.FromJson(response.Content).Data.ToList();

            return new List<TaxCodes>();

            //return LoadMockData();
        }

        private List<TaxCodes> LoadMockData()
        {
            var file = File.ReadAllText(@"Resources\TaxCodes.json");
            return TaxCodes.FromJson(file).Data.ToList();
        }

        public async Task<TaxCodes?> GetByName(string name)
        {
            var query = _credentials.BaseUri;
            var resource = "/api/common/taxcode" +
                           "?cols=sid,sbssid,taxname,isdefault" +
                           $"&filter=(sbssid,eq,{_subsidiary.SID})&sort=taxcode,asc";

            var response = await HttpClientFactory.InitializeAsync(query, resource, Method.GET);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var TaxCodeList = JsonConvert.DeserializeObject<OdataPrism<TaxCodes>>(response.Content).Data.ToList();


                return TaxCodeList.FirstOrDefault(x => x.TaxName.ToUpper() == name.ToUpper());
            }
            return new TaxCodes();
        }

        public IRestResponse AddTaxCodes()
        {
            List<TaxCodes> taxCodesList = new()
        {
            new TaxCodes("",_subsidiary.SID,0, "Taxable",true),
            new TaxCodes("",_subsidiary.SID,1, "Exempt",false),
            new TaxCodes("",_subsidiary.SID,2, "Luxury",false)
        };


            IRestResponse response = null;
            try
            {
                var query = _credentials.CommonUri;
                const string resource = "/taxcode";
                var body = CreateTaxCodesPayload(taxCodesList);

                return HttpClientFactory.InitializeAsync(query, resource, Method.POST, body).Result;

                //responses = "Error Message" + response.ErrorMessage + "\r\r \r\n " + body;
                //return false;
            }
            catch (Exception ex)
            {
                //responses = $"Response Error Message: {response.ErrorMessage} \r\n \r\n ex {ex.Message}";
            }

            return null;
        }

        private string CreateTaxCodesPayload(List<TaxCodes> taxCodesList)
        {
            var root = new OdataPrism<TaxCodes>
            {
                Data = taxCodesList,
            };

            return JsonConvert.SerializeObject(root);
        }

        //public static void InitialTaxCodes(AdministrationService generalSettings, out string message, out string status)
        //{
        //    try
        //    {
        //        // Add TaxCodes to Prism
        //        try
        //        {
        //            var response = generalSettings.AddTaxCodes();

        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                //message = $"Response Content: {response.Content} \r\r \r\n Request Body: \r\n {body}" ;
        //                var taxCodes = JsonConvert.DeserializeObject<OdataPrism<TaxCodes>>(response.Content).Data.ToList();
        //                if (taxCodes.Any())
        //                {
        //                    var logMessage = "Task Name: [Initial Tax Codes]\r\n" +
        //                              "Status: [Done]\r\n" +
        //                              $"Created Time: [Now] >> {DateTime.Now.Hour}:{DateTime.Now.Minute}";

        //                    message = logMessage.Replace("\\r\\n", "-").TrimStart('"').TrimEnd('"').TrimEnd('-').Split('-').ToString();

        //                }
        //            }
        //            else
        //            {
        //                message = $"An error occurred during Tax Codes initialization. \r\n \r\n {response.Content}";
        //                status = "An error occurred during Tax Codes initialization.";
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        //return BadRequest($"An error occurred during category synchronization: {e.Message}");
        //    }

        //    message = null;
        //    status = null;
        //}
    }
}
