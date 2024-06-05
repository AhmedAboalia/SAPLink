namespace SAPLink.Core.Connection;

public static class ConnectionStringFactory
{
    private static ServerInfo? _serverInfo;

    static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings\\Server.json");

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

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string dbDirectory = Path.Combine(baseDirectory, "Settings");

        // Ensure the database directory exists
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }


        return $"Data Source={dbDirectory}\\SAPLinkDB.db;";
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