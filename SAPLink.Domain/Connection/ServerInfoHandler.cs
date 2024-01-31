using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SAPLink.Domain.Connection
{
    public static class ServerInfoHandler
    {
        public static ServerInfo[] ReadConnectionStrings(string filePath)
        {
            EnsureFileExists(filePath);
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<ServerInfo[]>(json);
        }

        public static void SaveChanges(string filePath)
        {
            var connectionConfigs = ServerInfo.CreateDefaults();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(connectionConfigs, Formatting.Indented));
        }

        private static void EnsureFileExists(string filePath)
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            if (!File.Exists(filePath))
                SaveChanges(filePath);
        }
    }

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

            return new ServerInfo[]
            {
                new ServerInfo { Id = (int)Environments.Production, ConnectionName = "Prism Production Server Connection", Server = "KAFFARYRETAIL", Database = database, Password = "RetailTec@123", Active = pcName == "KAFFARYRETAIL" },
                new ServerInfo { Id = (int)Environments.Test, ConnectionName = "Prism (Test) SBS Test Server Connection", Server = "POSTEST", Database = database, Password = "RetailTec@123", Active = pcName == "POSTEST" },
                new ServerInfo { Id = (int)Environments.Local, ConnectionName = "Local (RTC) SBS Server Connection", Server = "ABOALIA", Database = database, Password = "P@ssw0rd", Active = pcName == "ABOALIA" }
            };
        }
    }

    public static class ConnectionStringFactory
    {
        private static ServerInfo _serverInfo;
        private static readonly string FilePath = "Database\\LocalSettings.json";

        private static string ToSQLServer() => $"Server={_serverInfo.Server};Database={_serverInfo.Database};TrustServerCertificate=True;User Id=sa;Password={_serverInfo.Password};";

        public static string SqlServer()
        {
            var connectionStrings = ServerInfoHandler.ReadConnectionStrings(FilePath);
            _serverInfo = connectionStrings?.FirstOrDefault(c => c.Active) ?? connectionStrings?.FirstOrDefault();
            return _serverInfo != null ? ToSQLServer() : "";
        }

        private static string ToSQLLite() => $"Data Source=Database\\{_serverInfo.Database}.db;";

        public static string SqlLite()
        {
            var connectionStrings = ServerInfoHandler.ReadConnectionStrings(FilePath);
            _serverInfo = connectionStrings?.FirstOrDefault(c => c.Active) ?? connectionStrings?.FirstOrDefault();
            return _serverInfo != null ? ToSQLLite() : "";
        }

        public static ServerInfo GetActiveConnection() => ServerInfoHandler.ReadConnectionStrings(FilePath)?.FirstOrDefault(c => c.Active);
    }

    public class LocalSettings
    {
        private static LocalSettings _setting;
        private static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\LocalSettings.txt");

        private static void SaveChanges(LocalSettings localSettings)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(localSettings));
        }

        public static void Init()
        {
            if (!File.Exists(path) || string.IsNullOrWhiteSpace(File.ReadAllText(path)))
            {
                _setting = new LocalSettings();
                SaveChanges(_setting);
                return;
            }

            _setting = JsonConvert.DeserializeObject<LocalSettings>(File.ReadAllText(path)) ?? new LocalSettings();
        }
    }
}
