using System.Diagnostics;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SAPLink.Utilities
{
    public static class Helper
    {
        public static async Task<bool> IsDashboardAvailable()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(3);
                try
                {
                    var cts = new CancellationTokenSource();
                    var timeoutTask = Task.Delay(httpClient.Timeout, cts.Token);
                    var requestTask = httpClient.GetAsync("https://localhost:44326/dashboard", cts.Token);

                    var completedTask = await Task.WhenAny(timeoutTask, requestTask);
                    if (completedTask == timeoutTask)
                    {
                        // Timeout occurred
                        cts.Cancel();
                        return false;
                    }

                    var response = await requestTask;
                    return response.IsSuccessStatusCode;

                    //using (var httpClient = new HttpClient())
                    //{
                    //    httpClient.Timeout = TimeSpan.FromSeconds(3);
                    //    var response = await httpClient.GetAsync("https://localhost:44326/dashboard");
                    //    return response.IsSuccessStatusCode;
                    //}
                }
                catch
                {
                    return false;
                }
            }
        }

        //TryKillProcess(KillSapLinkProcesses);

        public static void TryKillProcess()
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName("SAPLink"))
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
