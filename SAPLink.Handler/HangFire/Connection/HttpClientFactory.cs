using SAPLink.Core.Utilities;
using SAPLink.EF;
using SAPLink.Handler.Prism.Connection.Auth;
using System.Net;
using SAPLink.Core.Models;
using SAPLink.Core.Models.System;

namespace SAPLink.Handler.Connection;

public partial class HttpClientFactory<T> where T : class
{
    public static IRestResponse Initialize(string uri, string resource, Method method,
        IDictionary<string, string> headers, string body = "")
    {
        try
        {
            ApiClient = new RestClient(uri);

            Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Request.AddHeader(header.Key, header.Value);
                }
            }


            if (body.IsHasValue())
                Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            return ApiClient.Execute(Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        Request = null;
        return null;
    }

    public static IRestResponse InitializeIntegration(string Uri, string resource, Method method, string body = "")
    {
        try
        {
            ApiClient = new RestClient(Uri);

            Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            Request.AddHeader("Accept", "application/json");
            Request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            Request.AddHeader("Connection", "keep-alive");

            Request.AddHeader("Content-type", "application/json; charset=UTF-8");
            Request.AddHeader("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            Request.AddHeader("count", "true");


            if (body.IsHasValue())
                Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            return ApiClient.Execute(Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        //Request = null;
        return new RestResponse();
    }
}
