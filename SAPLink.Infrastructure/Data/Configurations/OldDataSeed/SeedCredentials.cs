namespace SAPLink.Infrastructure.Data.Configurations.OldDataSeed
{
    internal static class SeedCredentials
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            const string productionBaseUrl = "";

            const string testBaseUrl = "http://postest.alkaffary.com:8080";
            const string localBaseUrl = "http://194.163.155.105/api/backoffice";
            modelBuilder.Entity<Credentials>().HasData(new Credentials[]
      {
          new()
                {
                    EnvironmentCode = (int)Environments.Test,
                    EnvironmentName = "Test Environment",
                    Active = true,

                    PrismUserName = "sysadmin",
                    PrismPassword = "sysadmin",
                    BaseUri = testBaseUrl,
                    BackOfficeUri = $"{testBaseUrl}/api/backoffice",
                    CommonUri = $"{testBaseUrl}/v1/rest",
                    RestUri = $"{testBaseUrl}/api/common",
                    Origin = testBaseUrl,
                    Referer = $"{testBaseUrl}/prism.shtml",
                    AuthSession = "34B7701F5A264DD6BAFD6CC35A68A195",

                    ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
                    Server = "SAP-TEST",
                    ServerTypes = BoDataServerTypes.dst_MSSQL2016,
                    CompanyDb = "TESTDB",
                    UserName = "manager",
                    Password = "Ag123654*",
                    DbUserName = "sa",
                    DbPassword = "sap123456*",
                    AuthUserName = @"{{""UserName"" : ""manager"",""CompanyDB"" : ""TESTDB""}}",
                    AuthPassword = "Qw123654*",
                    Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpRdzEyMzY1NCo=",
                    Cookie = ""
                },
                new()
                {
                    EnvironmentCode = (int)Environments.Local,
                    EnvironmentName = "Local Environment",
                    Active = false,

                    PrismUserName = "sysadmin",
                    PrismPassword = "sysadmin",
                    BaseUri = localBaseUrl,
                    BackOfficeUri = $"{localBaseUrl}/api/backoffice",
                    CommonUri = $"{localBaseUrl}/v1/rest",
                    RestUri = $"{localBaseUrl}/api/common",
                    Origin = localBaseUrl,
                    Referer = $"{localBaseUrl}/prism.shtml",
                    AuthSession = "3AE6391AFFA54F9EAE8175C78FDC01D8",

                    ServiceLayerUri = "https://Localhost:50000/b1s/v1/",
                    Server = "ABOALIA",
                    ServerTypes = BoDataServerTypes.dst_MSSQL2019,
                    CompanyDb = "SBODemoGB",
                    UserName = "manager",
                    Password = "manager",
                    DbUserName = "sa",
                    DbPassword = "P@ssw0rd",
                    AuthUserName = @"{{""UserName"" : ""manager"",""CompanyDB"" : ""SBODemoGB""}}",
                    AuthPassword = "manager",
                    Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJTQk9EZW1vR0IifTptYW5hZ2Vy",
                    Cookie = ""
                },
            });
        }
    }
}
