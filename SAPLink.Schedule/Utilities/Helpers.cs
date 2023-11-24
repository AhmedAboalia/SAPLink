using System.Diagnostics;

namespace SAPLink.Schedule.Utilities
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

        public static string To24HourFormat(this string time)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(time, "h:mm tt", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                return dateTime.ToString("HH:mm");
            }
            else
            {
                throw new FormatException("Invalid 12-hour time format!");
            }
        }
        public static (int Hours, int Minutes) To24HourMinutesFormat(this string twelveHourTime)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(twelveHourTime, "h:mm tt", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                return (dateTime.Hour, dateTime.Minute);
            }
            else
            {
                throw new FormatException("Invalid 12-hour time format!");
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
