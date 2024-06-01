using SAPLink.Core.Models.System;
using SAPLink.EF;
using SAPLink.EF.Data;

namespace SAPLink.Handler.Connection
{
    public static partial class HttpClientFactory
    {
        private static RestClient ApiClient { get; set; }
        public static string LastErrorMessage { get; private set; }
        private static RestRequest Request { get; set; }

        //private static readonly ApplicationDbContext Context = new();
        //private static readonly UnitOfWork UnitOfWork = new(Context);

        //static readonly string[] includes = { "Credentials", "Credentials.Subsidiaries" };
        //static readonly Clients Client = UnitOfWork.Clients.FindAsync(c => c.Active == true, includes).Result;

        //private static readonly Credentials Credential = Client.Credentials.FirstOrDefault();
        //private static Subsidiaries subsidiary = Credential.Subsidiaries.FirstOrDefault();

    }
}