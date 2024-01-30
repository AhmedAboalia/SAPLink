using SAPLink.Domain.Utilities;
using SAPLink.Handler.Prism.Connection.Auth;

namespace SAPLink.Application.Connection;

public static partial class HttpClientFactory
{
    public static async Task<IRestResponse> InitializeAsync(string Uri, string resource, Method method, string body = "")
    {
        try
        {
            Application.Connection.HttpClientFactory.ApiClient = new RestClient(Uri);
            RefreshAuthSession();

            Application.Connection.HttpClientFactory.Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            Application.Connection.HttpClientFactory.Request.AddHeader("Accept", "application/json, text/plain, version=2");
            Application.Connection.HttpClientFactory.Request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            Application.Connection.HttpClientFactory.Request.AddHeader("Auth-Session", Application.Connection.HttpClientFactory.Credential.AuthSession);
            Application.Connection.HttpClientFactory.Request.AddHeader("Connection", "keep-alive");

            Application.Connection.HttpClientFactory.Request.AddHeader("Content-type", "application/json; charset=UTF-8");
            Application.Connection.HttpClientFactory.Request.AddHeader("Origin", Application.Connection.HttpClientFactory.Credential.Origin);
            Application.Connection.HttpClientFactory.Request.AddHeader("Referer", Application.Connection.HttpClientFactory.Credential.Referer);
            Application.Connection.HttpClientFactory.Request.AddHeader("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");

            if (body.IsHasValue())
                Application.Connection.HttpClientFactory.Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            if (Application.Connection.HttpClientFactory.Request != null)
            {
                var response = await Application.Connection.HttpClientFactory.ApiClient.ExecuteAsync(Application.Connection.HttpClientFactory.Request);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    
                    response = await Application.Connection.HttpClientFactory.ApiClient.ExecuteAsync(Application.Connection.HttpClientFactory.Request);
                }
                return response;
            }
            
        }
        catch (Exception e)
        {
            // Handle the exception appropriately
        }

        //Request = new RestRequest();

        return new RestResponse();
    }
    private static async void RefreshAuthSession()
    {
        var newAuth = await LoginManager.GetAuthSessionAsync(Application.Connection.HttpClientFactory.Credential.BaseUri, Application.Connection.HttpClientFactory.Credential.PrismUserName, Application.Connection.HttpClientFactory.Credential.PrismPassword);
        if (newAuth.IsHasValue())
        {
            Application.Connection.HttpClientFactory.Credential.AuthSession = newAuth;

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
    public static IRestResponse Initialize(string uri, string resource, Method method,
        IDictionary<string, string> headers, string body = "")
    {
        try
        {
            Application.Connection.HttpClientFactory.ApiClient = new RestClient(uri);

            Application.Connection.HttpClientFactory.Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Application.Connection.HttpClientFactory.Request.AddHeader(header.Key, header.Value);
                }
            }


            if (body.IsHasValue())
                Application.Connection.HttpClientFactory.Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            return Application.Connection.HttpClientFactory.ApiClient.Execute(Application.Connection.HttpClientFactory.Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        Application.Connection.HttpClientFactory.Request = null;
        return null;
    }

    public static IRestResponse InitializeIntegration(string Uri, string resource, Method method, string body = "")
    {
        try
        {
            Application.Connection.HttpClientFactory.ApiClient = new RestClient(Uri);

            Application.Connection.HttpClientFactory.Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            Application.Connection.HttpClientFactory.Request.AddHeader("Accept", "application/json");
            Application.Connection.HttpClientFactory.Request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            Application.Connection.HttpClientFactory.Request.AddHeader("Connection", "keep-alive");

            Application.Connection.HttpClientFactory.Request.AddHeader("Content-type", "application/json; charset=UTF-8");
            Application.Connection.HttpClientFactory.Request.AddHeader("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            Application.Connection.HttpClientFactory.Request.AddHeader("count", "true");


            if (body.IsHasValue())
                Application.Connection.HttpClientFactory.Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            return Application.Connection.HttpClientFactory.ApiClient.Execute(Application.Connection.HttpClientFactory.Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        //Request = null;
        return new RestResponse();
    }
}
