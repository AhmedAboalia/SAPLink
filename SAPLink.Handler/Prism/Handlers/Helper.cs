using SAPLink.Core.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPLink.Handler.Prism.Handlers
{
    public class Helper
    {
        public static ILogger CreateLoggerConfiguration(string document, string type, LogsTypes logsType = LogsTypes.GeneralLogs)
        {
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\{DateTime.Now:dd-MM-yyy}\\";//DateTime.Now:D  //DateTime.Now:dd-MM-yyy dddd}

            if (logsType == LogsTypes.InboundData)
                path += $"Inbound Data\\{document}\\";
            else if (logsType == LogsTypes.OutboundData)
                path += $"Outbound Data\\{document}\\";
            
            path.EnsureDirectoryExists();

            var fileName = $@"{document} {type}.log";  //- {DateTime.Now:yy-MM-dd}

            var filePath = $@"{path}{fileName}";

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(filePath,
                    outputTemplate: "{Timestamp:MM/dd/yyyy HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            logger.Information($"--------------------< {document} {type} Logs >--------------------");
            logger.Information($"--------------------< {DateTime.Now:g} >--------------------");

            return logger;
        }
    }

    public enum LogsTypes
    {
        InboundData,
        OutboundData,
        GeneralLogs
    }
}
