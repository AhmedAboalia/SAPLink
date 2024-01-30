namespace SAPLink.Domain.Connection;

public static class ConnectionStringFactory
{
    private static ServerInfo? _serverInfo;

    private static readonly string filePath = "Database\\LocalSettings.json";

    //static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings\\LocalSettings.txt");

    private static string ToSQLServer()
    {
        return $"Server={_serverInfo.Server};Database={_serverInfo.Database};TrustServerCertificate=True;User Id=sa;Password={_serverInfo.Password};";
    }
    public static string SqlServer()
    {
        ServerInfo?[]? connectionStrings = ServerInfoHandler.ReadConnectionStrings(filePath);

        if (connectionStrings != null)
        {
            foreach (var connection in connectionStrings)
            {
                if (!connection.Active) continue;

                _serverInfo = connection;
                return ToSQLServer();
            }
        }
        else
        {
            var serverInfos = ServerInfoHandler.ReadConnectionStrings(filePath);
            if (serverInfos == null || !serverInfos.Any())
            {
                ServerInfoHandler.SaveChanges(filePath);
                SqlServer();
            }
        }

        return "";
    }

    private static string ToSQLLite()
    {
        return $"Data Source=Database\\{_serverInfo.Database}.db;";
    }

    public static string SqlLite()
    {
        ServerInfo?[]? connectionStrings = ServerInfoHandler.ReadConnectionStrings(filePath);

        if (connectionStrings != null)
        {
            foreach (var connection in connectionStrings)
            {

                _serverInfo = connection;

                if (connection.Active)
                    return ToSQLLite();
            }
        }
        else
        {
            var serverInfo = ServerInfoHandler.ReadConnectionStrings(filePath);
            if (serverInfo == null || !serverInfo.Any())
            {
                ServerInfoHandler.SaveChanges(filePath);
                SqlLite();
            }
        }

        return "";
    }

    public static ServerInfo GetActiveConnection()
    {
        ServerInfo?[]? connectionStrings = ServerInfoHandler.ReadConnectionStrings(filePath);

        if (connectionStrings != null)
        {
            foreach (var connection in connectionStrings)
            {
                if (connection.Active)
                    return _serverInfo = connection;
            }
        }

        return null;
    }
}

public class LocalSettings
{
    private static LocalSettings Setting;

    static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\LocalSettings.txt");


    private static void SaveChanges(LocalSettings localSettings)
    {
        try
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(localSettings));
        }
        catch (Exception ex)
        {

        }
    }

    public static void Init()
    {
        try
        {
            if (!File.Exists(path))
            {
                Setting = new LocalSettings();
                using StreamWriter streamWriter = new StreamWriter(path, append: true);
                streamWriter.Write(JsonConvert.SerializeObject(Setting));
                return;
            }
            string value = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(value))
            {
                Setting = new LocalSettings();
                SaveChanges(Setting);
                return;
            }
            Setting = JsonConvert.DeserializeObject<LocalSettings>(value);
            if (Setting == null)
            {
                //PV.ShowNotification("فشل قراءة الإعدادات المحلية");
                Setting = new LocalSettings();
            }
        }
        catch (Exception ex)
        {
            //PV.error.SaveError(ex);
        }
    }
}