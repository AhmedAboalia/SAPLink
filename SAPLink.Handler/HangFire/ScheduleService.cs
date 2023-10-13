
using SAPLink.EF;
using SAPLink.Core;
using SAPLink.Core.Models.System;
using SAPLink.Handler.Connection;
using HttpClientFactory = SAPLink.Handler.Connection.HttpClientFactory;
using static SAPLink.Core.InboundEnums;

namespace SAPLink.Handler.Integration
{
    public static class ScheduleService
    {
        #region Schedule It Old

        static void ScheduleItOld(Recurrence recurrence)
        {
            try
            {
                var resource = recurrence.Document.ToString();
                var body = @"
                                   {
                                    ""document"": """ + recurrence.Document + @""",
                                    ""recurring"": """ + recurrence.Recurring + @""",
                                    ""interval"":""" + recurrence.Interval + @""" 
                                   }";


                var response = HttpClientFactory.InitializeIntegration("https://localhost:7208/api/v1/Inventory/", resource, Method.POST,
                    @"{
                        ""document"": ""Items"",
                        ""recurring"": ""Hourly"",
                        ""interval"": 0
                   }");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Recurrence recurrences = JsonConvert.DeserializeObject<Recurrence>(response.Content);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        public static void Run(Recurrence recurrence, out string message)
        {
            var messageLog = "";
            var body = recurrence.Recurring == Enums.Repeats.Hourly
                ? PrepareHourlyRequestBody(recurrence)
                : PrepareDailyRequestBody(recurrence);

            string Uri = recurrence.Recurring == Enums.Repeats.Hourly
                ? PrepareHourlyRequestUri(recurrence)
                : PrepareDailyRequestUri(recurrence);

            var response = HttpClientFactory.InitializeIntegration(Uri, "", Method.POST, body);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var FullText = response.Content.Replace("\\r\\n", "-").TrimStart('"').TrimEnd('"').TrimEnd('-');
                messageLog = FullText.Split('-').ToString();
            }

            message = messageLog;
        }

        static string PrepareHourlyRequestUri(Recurrence recurrence)
        {
            return $"https://localhost:7208/api/v1/Inventory/{recurrence.Document}/SyncEveryHour";
        }

        static string PrepareDailyRequestUri(Recurrence recurrence)
        {
            return $"https://localhost:7208/api/v1/Inventory/{recurrence.Document}/SyncEveryDayOfWeek";
        }

        static string PrepareHourlyRequestBody(Recurrence recurrence)
        {
            return @"
                        {
                         ""document"": """ + recurrence.Document + @""",
                         ""interval"":""" + recurrence.Interval + @""" 
                        }";
        }

        static string PrepareDailyRequestBody(Recurrence recurrence)
        {
            return @"
                        {
                         ""document"": """ + recurrence.Document + @""",
                         ""dayOfWeek"":""" + recurrence.DayOfWeek + @""" 
                        }";
        }

        public static Recurrence GetRecurrence(UnitOfWork unitOfWork, int selectedIndex, Recurrence recurrence)
        {
            switch (selectedIndex)
            {
                case (int)Documents.Departments:
                    recurrence = unitOfWork.Recurrences.Find(x => x.Document == Documents.Departments);
                    break;

                case (int)Documents.Items:
                    recurrence = unitOfWork.Recurrences.Find(x => x.Document == Documents.Items);
                    break;

                case (int)Documents.Vendors:
                    recurrence = unitOfWork.Recurrences.Find(x => x.Document == Documents.Vendors);
                    break;
            }

            return recurrence;
        }

    }
}
