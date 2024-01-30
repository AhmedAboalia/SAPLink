
namespace SAPLink.Domain.Connection;

public static class ServerInfoHandler
{
    public static ServerInfo[] ReadConnectionStrings(string filePath)
    {
        if (!File.Exists(filePath))
        {
            EnsureDirectoryExists(filePath);
            SaveChanges(filePath);
        }

        var json = File.ReadAllText(filePath);

        return JsonConvert.DeserializeObject<ServerInfo[]>(json);
    }

    public static void SaveChanges(string filePath)
    {
        var connectionConfigs = ServerInfo.CreateDefaults();
        string json = JsonConvert.SerializeObject(connectionConfigs, Formatting.Indented);

        File.WriteAllText(filePath, json);
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        var directoryName = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }

}