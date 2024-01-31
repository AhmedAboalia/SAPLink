﻿using SAPLink.Application.SAP.Connection;
using SAPLink.Domain.System;
using SAPLink.Domain.Utilities;

namespace SAPLink.Application.Connection;

public partial class HttpClientFactory
{

    private static Credentials Credential = null;
    public static IRestResponse Initialize(string resource, Method method, Clients client, LoginModel.LoginTypes LoginTypes = LoginModel.LoginTypes.Basic, LoginModel LoginData = null, string body = "",
        bool applyPaging = false, int maxPerPage = 250, string contentType = "")
    {
        try
        {
            Credential = client.Credentials.FirstOrDefault();
            ApiClient = new RestClient(Credential.ServiceLayerUri);

            HttpClientFactory.Request = new RestRequest
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
