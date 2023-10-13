

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

        private static readonly ApplicationDbContext Context = new();
        private static readonly UnitOfWork UnitOfWork = new(Context);

        static readonly string[] includes = { "Credentials", "Credentials.Subsidiaries" };
        static readonly Clients Client = UnitOfWork.Clients.FindAsync(c => c.Active == true, includes).Result;

        private static readonly Credentials Credential = Client.Credentials.FirstOrDefault();
        private static Subsidiaries subsidiary = Credential.Subsidiaries.FirstOrDefault();


        public static void MakeCert()
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(10));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("c:\\temp\\mycert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText("c:\\temp\\mycert.cer",
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }

    }
}