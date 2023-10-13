namespace SAPLink.Core.Connection;

public class ServerInfo
{
    public int Id { get; set; }
    public string ConnectionName { get; set; }
    public string Server { get; set; }
    public string Database { get; set; }
    public string Password { get; set; }
    public bool Active { get; set; }
    public static ServerInfo[] CreateDefaults()
    {
        string pcName = Environment.MachineName.ToUpper();
        string database = "SAPLinkDB";

        var serverlist = new ServerInfo[]
        {
            //new()
            //{
            //    Id = (int)Environments.Production,
            //    ConnectionName = "Prism Production Server Connection",
            //    Server = "KAFFARYRETAIL",
            //    Database = database,
            //    Password = "RetailTec@123",
            //    Active = pcName == "KAFFARYRETAIL",
            //},
            new()
            {
                Id = (int)Environments.Test,
                ConnectionName = "Prism (Test) SBS Test Server Connection",
                Server = "POSTEST",
                Database = database,
                Password = "RetailTec@123",
                Active = pcName == "POSTEST",
            },
            new()
            {
                Id = (int)Environments.Local,
                ConnectionName = "Local (RTC) SBS Server Connection",
                Server = "ABOALIA",
                Database = database,
                Password = "P@ssw0rd",
                Active = pcName == "ABOALIA",
            },
            //new()
            //{
            //    ConnectionName = "SAP Server Connection",
            //    Server = "SAP-TEST",
            //    Database = database,
            //    Password = "sap123456*",
            //    Active = pcName == "SAP-TEST",
            //},
        };

        return serverlist;
    }

}