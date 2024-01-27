using SAPLink.Core.Models;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.EF;
using SAPLink.EF.Data;
using SAPLink.Handler.Prism.Connection.Auth;

namespace SAPLink.Handler.Connection
{
    public partial class HttpClientFactory<T> where T : class
    {
        private static RestClient ApiClient { get; set; }
        public static string LastErrorMessage { get; private set; }
        private static RestRequest Request { get; set; }

        private static ApplicationDbContext Context = new();
        private static UnitOfWork UnitOfWork = new(Context);

        static string[] includes = new[] { "Credentials", "Credentials.Subsidiaries" };
        private static Clients Client = UnitOfWork.Clients.FindAsync(c => c.Active == true, includes).Result;

        private static Credentials Credential = Client.Credentials.FirstOrDefault();
        //private static Subsidiaries subsidiary = Credential.Subsidiaries.FirstOrDefault();

    
        public static async Task<Responses> InitializeAsync(string Uri, string resource, Method method, string body = "", int pageNo = 1)
        {
            try
            {
                ApiClient = new RestClient(Uri);

                RefreshAuthSession();

                var pageSize = 30;
                resource += $"&count=true&page_no={pageNo}&page_size={pageSize}";
                Request = new RestRequest
                {
                    Resource = resource,
                    Method = method,
                    Timeout = -1
                };

                Request.AddHeader("Accept", "application/json, text/plain, version=2");
                Request.AddHeader("Accept-Language", "en-US,en;q=0.9");
                Request.AddHeader("Auth-Session", Credential.AuthSession);
                Request.AddHeader("Connection", "keep-alive");

                Request.AddHeader("Content-type", "application/json; charset=UTF-8");
                Request.AddHeader("Origin", Credential.Origin);
                Request.AddHeader("Referer", Credential.Referer);
                Request.AddHeader("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");

                if (body.IsHasValue())
                    Request.AddParameter("application/json", body, ParameterType.RequestBody);

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;

                if (Request != null)
                {
                    var responses = new Responses();
                    responses.Response = await ApiClient.ExecuteAsync(Request);

                    if (responses.Response.StatusCode == HttpStatusCode.Forbidden)
                    {

                        responses.Response = await ApiClient.ExecuteAsync(Request);
                    }

                    if (responses.Response.Headers.Any(h => h.Name == "Contentrange"))
                    {
                        var Contentrange = responses.Response.Headers.First(h => h.Name == "Contentrange").Value.ToString();

                        responses.GetContentRange(Contentrange, pageSize);
                    }

                    return responses;
                }

            }
            catch (Exception e)
            {
                // Handle the exception appropriately
            }

            return new Responses();
        }
        private static async void RefreshAuthSession()
        {
            var newAuth = await LoginManager.GetAuthSessionAsync(Credential.BaseUri, Credential.PrismUserName, Credential.PrismPassword);
            if (newAuth.IsHasValue())
            {
                Credential.AuthSession = newAuth;

                //UnitOfWork.Credentials.Update(Credential);
                //UnitOfWork.SaveChanges();

                //using (var context = new UnitOfWork(Context))
                //{
                //    context.Credentials.Update(Credential);
                //    context.SaveChanges();
                //}
            }
            else
                RefreshAuthSession();
        }
    }
}