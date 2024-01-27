using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using SAPLink.Core.Models.System;
using SAPLink.Core.Utilities;
using SAPLink.EF.Data;
using SAPLink.EF;
using SAPLink.Handler.SAP.Connection;
using SAPLink.Core.Models;

namespace SAPLink.Handler.Connection;

public partial class SAPHttpClientFactory
{
    private static RestClient ApiClient { get; set; }
    public static string LastErrorMessage { get; private set; }
    private static RestRequest Request { get; set; }

    private ApplicationDbContext Context = new();
    private UnitOfWork UnitOfWork = null;

    static string[] includes = null;
    private static Clients Client = null;

    private static Credentials Credential = null;
    //private static Subsidiaries subsidiary = Credential.Subsidiaries.FirstOrDefault();

    public SAPHttpClientFactory()
    {
        Context = new();
        UnitOfWork = new(Context);
        includes = new[] { "Credentials", "Credentials.Subsidiaries" };

        Client = UnitOfWork.Clients.FindAsync(c => c.Active == true, includes).Result;
        Credential = Client.Credentials.FirstOrDefault();
    }
    public static Responses Initialize(string resource, Method method,Clients clients,
        LoginModel.LoginTypes LoginTypes = LoginModel.LoginTypes.Basic, LoginModel LoginData = null, string body = "",
        bool applyPaging = false, int maxPerPage = 250, string contentType = "")
    {
        try
        {
            Client = clients;
            Credential = Client.Credentials.FirstOrDefault();
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

            return (Responses)ApiClient.Execute(Request);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
        }

        Request = null;
        return null;
    }
}
