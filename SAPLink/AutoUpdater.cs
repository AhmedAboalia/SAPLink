using System.Diagnostics;
using System.IO.Compression;

namespace SAPLink;

public class AutoUpdater
{
    static string updateServerUrl = "https://onedrive.live.com/?cid=1EFF9D573DB2C73F&id=1EFF9D573DB2C73F%21462678&parId=1EFF9D573DB2C73F%21462672&o=OneUp";
    static string versionFileUrl = "https://pastebin.com/raw/Ykduzjrs";
    static string appPath = AppDomain.CurrentDomain.BaseDirectory;


    public static void CheckForUpdates()
    {
        // Retrieve the version from the server
        Version serverVersion = GetServerVersion();

        // Compare with the installed version
        Version localVersion = GetCurrentVersion();

        if (serverVersion > localVersion)
        {
            Console.WriteLine("New version available. Updating...");

            // Download and apply updates
            DownloadAndApplyUpdates();

            // Restart the application
            RestartApplication();
        }
        else
        {
            Console.WriteLine("No updates available.");
        }
    }

    static Version GetServerVersion()
    {
        // Retrieve the Git version from the text file on the server
        string versionString = GetGitVersionFromTextFile(versionFileUrl);

        if (Version.TryParse(versionString, out Version serverVersion))
        {
            return serverVersion;
        }
        else
        {
            // Handle the case where the version cannot be parsed
            throw new InvalidOperationException("Invalid version format in the text file.");
        }
    }
    static string GetGitVersionFromTextFile(string fileUrl)
    {
        using (WebClient client = new WebClient())
        {
            // Download the text file containing the Git version
            string fileContent = client.DownloadString(fileUrl);

            // Return the raw content of the text file
            return fileContent;
        }
    }

    static Version GetCurrentVersion()
    {
        // Get the version from the currently running assembly
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }

    static void DownloadAndApplyUpdates()
    {
        using (WebClient client = new WebClient())
        {
            // Download the update zip file
            client.DownloadFile(updateServerUrl, "update.zip");
        }

        // Extract the contents of the zip file to the application directory
        ZipFile.ExtractToDirectory("update.zip", appPath);

        // Clean up - delete the zip file
        File.Delete("update.zip");
    }

    static void RestartApplication()
    {
        // Restart the application
        Process.Start(new ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = "YourAppName.exe",
            Verb = "runas"  // Run as administrator if needed
        });

        // Close the current instance of the application
        Environment.Exit(0);
    }
}