using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.Handler.SAP.Connection;

namespace SAPLink.Handler.Connection;

public partial class HttpClientFactory<T> where T : class
{
    public static IRestResponse Initialize(string resource, Method method, LoginModel.LoginTypes LoginTypes = LoginModel.LoginTypes.Basic, LoginModel LoginData = null, string body = "",
        bool applyPaging = false, int maxPerPage = 250, string contentType = "")
    {
        try
        {
            ApiClient = new RestClient(Credential.ServiceLayerUri);

            Request = new RestRequest
            {
                Resource = resource,
                Method = method,
                Timeout = -1
            };

            if (LoginData != null && LoginData.LoginType == LoginModel.LoginTypes.Standard)
            {
                body = $@"{{""UserName"" : ""{LoginData.UserName}"",""Password"" : ""{LoginData.Password}"",""CompanyDB"" : ""{LoginData.Company}""}}";
            }
            else
            {
                Request.AddHeader("Authorization", Credential.Authorization);
                //Request.AddHeader("Cookie", credentials.Cookie);
            }

            if (contentType.IsNullOrEmpty())
                Request.AddHeader("Content-type", "application/json");
            else
                Request.AddHeader("Content-type", contentType);

            if (applyPaging)
                Request.AddHeader("Prefer", $"odata.maxpagesize={maxPerPage}");


            if (body.IsHasValue())
                Request.AddParameter("application/json", body, ParameterType.RequestBody);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
            //MakeCert();

            return ApiClient.Execute(Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        Request = null;
        return null;
    }
}
