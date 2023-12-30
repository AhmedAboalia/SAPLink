namespace SAPLink.EF.Data.Configurations;

internal class CredentialsConfiguration : IEntityTypeConfiguration<Credentials>
{
    public void Configure(EntityTypeBuilder<Credentials> builder)
    {
        builder.ToTable("Credentials");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).IsRequired();

        builder.HasOne(cr => cr.Client)
            .WithMany(c => c.Credentials)
            .HasForeignKey(cr => cr.ClientId)
            .IsRequired();

        builder.HasMany(cr => cr.Subsidiaries)
            .WithOne(s => s.Credential)
            .HasForeignKey(s => s.CredentialId)
            .IsRequired();

        builder.HasData(CredentialsList.GetCredentials());
    }
}
internal static class CredentialsList
{
    const string ProductionBaseUrl = "http://kaffaryretail.alkaffary.com:8080";

    const string TestBaseUrl = "http://postest.alkaffary.com:8080";
    const string LocalBaseUrl = "http://194.163.155.105";
    const string integrationUrl = "http://saplink.alkaffary.com";

    public static Credentials[] GetCredentials()
    {
        return new Credentials[]
        {
            new()
            {
                Id = (int)Environments.Production,
                ClientId = (int)Environments.Production,
                EnvironmentCode = (int)Environments.Production,
                EnvironmentName = "Production Environment",
                Active = false,

                PrismUserName = "SAPLINK",
                PrismPassword = "RetailTec@123",
                BaseUri = ProductionBaseUrl,
                BackOfficeUri = $"{ProductionBaseUrl}/api/backoffice",
                CommonUri = $"{ProductionBaseUrl}/v1/rest",
                RestUri = $"{ProductionBaseUrl}/api/common",
                Origin = ProductionBaseUrl,
                Referer = $"{ProductionBaseUrl}/prism.shtml",
                AuthSession = "",

                ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
                Server = "SAP-TEST",
                ServerTypes = BoDataServerTypes.dst_MSSQL2016,
                CompanyDb = "",
                UserName = "manager",
                Password = "",
                DbUserName = "sa",
                DbPassword = "sap123456*",

                AuthUserName = @"{{""UserName"" : ""manager"",""CompanyDB"" : """"}}",
                AuthPassword = "",
                Authorization = "",
                Cookie = "",

                IntegrationUrl = integrationUrl,
                ActiveLog = false,
            },
            new()
            {
                Id = (int)Environments.Test,
                ClientId = (int)Environments.Test, // Foreign key referencing the client
                EnvironmentCode = (int)Environments.Test,
                EnvironmentName = "Test Environment",
                Active = false,

                PrismUserName = "SAPLINK",//;"sysadmin",
                PrismPassword = "RetailTec@123",//"kaf@admin",
                BaseUri = TestBaseUrl,

                BackOfficeUri = $"{TestBaseUrl}/api/backoffice",
                CommonUri = $"{TestBaseUrl}/v1/rest",
                RestUri = $"{TestBaseUrl}/api/common",
                Origin = TestBaseUrl,
                Referer = $"{TestBaseUrl}/prism.shtml",
                AuthSession = "369B7B1BF58F469896B06B804BFBE272",

                ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
                Server = "SAP-TEST",
                ServerTypes = BoDataServerTypes.dst_MSSQL2016,
                CompanyDb = "TESTDB",
                UserName = "manager",
                Password = "Ag123456*",//Qw123654*
                DbUserName = "sa",
                DbPassword = "sap123456*",

                AuthUserName = @"{{""UserName"" : ""manager"",""CompanyDB"" : ""TESTDB""}}",
                AuthPassword = "Ag123456*",//Qw123654*
                Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpBZzEyMzQ1Nio=",
                Cookie = "",

                IntegrationUrl = integrationUrl,
                ActiveLog = false,

            },
            //new()
            //{
            //    Id = (int)Environments.Test,
            //    ClientId = (int)Environments.Test, // Foreign key referencing the client
            //    EnvironmentCode = (int)Environments.Test,
            //    EnvironmentName = "Test Environment",
            //    Active = false,

            //    PrismUserName = "sysadmin",
            //    PrismPassword = "sysadmin",
            //    BaseUri = TestBaseUrl,

            //    BackOfficeUri = $"{TestBaseUrl}/api/backoffice",
            //    CommonUri = $"{TestBaseUrl}/v1/rest",
            //    RestUri = $"{TestBaseUrl}/api/common",
            //    Origin = TestBaseUrl,
            //    Referer = $"{TestBaseUrl}/prism.shtml",
            //    AuthSession = "369B7B1BF58F469896B06B804BFBE272",

            //    ServiceLayerUri = "https://sap-test:50000/b1s/v1/",
            //    Server = "SAP-TEST",
            //    ServerTypes = BoDataServerTypes.dst_MSSQL2016,
            //    CompanyDb = "TESTDB",
            //    UserName = "manager",
            //    Password = "Rs123456*",//Qw123654*
            //    DbUserName = "sa",
            //    DbPassword = "sap123456*",

            //    AuthUserName = @"{{""UserName"" : ""manager"",""CompanyDB"" : ""TESTDB""}}",
            //    AuthPassword = "Rs123456*",//Qw123654*
            //    Authorization = "Basic eyJVc2VyTmFtZSI6ICJtYW5hZ2VyIiwgIkNvbXBhbnlEQiI6ICJURVNUREIifTpSczEyMzQ1Nio=",
            //    Cookie = "",

            //    IntegrationUrl = integrationUrl,
            //},
            new()
            {
                Id = (int)Environments.Local,
                ClientId = (int)Environments.Local, // Foreign key referencing the client
                EnvironmentCode = (int)Environments.Local,
                EnvironmentName = "Local Environment",
                Active = true,

                PrismUserName = "sysadmin",
                PrismPassword = "sysadmin",
                BaseUri = LocalBaseUrl,
                BackOfficeUri = $"{LocalBaseUrl}/api/backoffice",
                CommonUri = $"{LocalBaseUrl}/v1/rest",
                RestUri = $"{LocalBaseUrl}/api/common",
                Origin = LocalBaseUrl,
                Referer = $"{LocalBaseUrl}/prism.shtml",
                AuthSession = "F1726B4EC6304D969ED816D844617C02",

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
                Cookie = "",

                IntegrationUrl = integrationUrl,
                ActiveLog = false,
            },
        };
    }
}