using SAPLink.Core.Utilities;
using SAPLink.Handler.Integration;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Handler.SAP.Handlers;
using ServiceLayerHelper.RefranceModels;

namespace SAPLink.API.Controllers;

[Route("api/v1/Inventory/[controller]")]
[ApiController]
public class ScheduleSyncs : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    private readonly Clients _client;
    private readonly DepartmentService _departmentService;
    private readonly DepartmentsHandler _departmentsHandler;
    private readonly VendorsService _vendorsService;
    private readonly VendorsHandler _vendorsHandler;
    private readonly ServiceLayerHandler _serviceLayer;
    private readonly ItemsService _itemsService;
    private readonly ItemsHandler _itemsHandler;

    public ScheduleSyncs(
        ApplicationDbContext context,
        UnitOfWork unitOfWork,
        Clients client,
        DepartmentService departmentService,
        DepartmentsHandler departmentsHandler,
        VendorsService vendorsService,
        VendorsHandler vendorsHandler,
        ServiceLayerHandler serviceLayer,
        ItemsService itemsService,
        ItemsHandler itemsHandler)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _client = client;
        _departmentService = departmentService;
        _departmentsHandler = departmentsHandler;
        _vendorsService = vendorsService;
        _vendorsHandler = vendorsHandler;
        _serviceLayer = serviceLayer;
        _itemsService = itemsService;
        _itemsHandler = itemsHandler;
    }

    /// <summary>
    /// Returns Recurrence that will fires every hour at the specified minute.
    /// </summary>
    /// <param name="recurrence">'Document' Valid value is ('Departments' or 'Items' or 'Vendors') - 'Interval' valid Value between (0-59) Minutes.</param>
    /// <returns>Recurrence</returns>
    [HttpPost("Hourly")]
    public async Task<IActionResult> Hourly([FromBody] RecurrenceHourlyDto recurrence)
    {
        if (recurrence.Interval > 59)
        {
            return BadRequest(
                "Invalid 'interval' with current request, update to 'Interval' to be between (0-59).");
        }
        
        switch (recurrence.Document)
        {
            case Documents.Departments:
                {
                    string Minutes = recurrence.Interval.ToString();
                    Minutes = Minutes.Length != 2 ? "0" + recurrence.Interval : recurrence.Interval.ToString();

                    RecurringJob.AddOrUpdate(
                        $"Sync {recurrence.Document} in an hour at {DateTime.Now.Hour + 1}:{Minutes} minutes.",
                        () => SyncDepartments(), Cron.Hourly(recurrence.Interval));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Execution Time: [in an hour] at {DateTime.Now.Hour + 1}:{Minutes} minutes.\r\n");

                    //return Ok(new TaskDetails("Started", "[Sync {recurrence.Document}]", $"{DateTime.Now.Hour}:{DateTime.Now.Minute}", $"in an hour at {DateTime.Now.Hour + 1}:{Minutes} minutes."));
                }
            case Documents.Items:
                {
                    string Minutes = recurrence.Interval.ToString();
                    Minutes = Minutes.Length != 2 ? "0" + recurrence.Interval : recurrence.Interval.ToString();

                    RecurringJob.AddOrUpdate($"Sync {recurrence.Document} in an hour at {DateTime.Now.Hour + 1}:{Minutes} minutes.",
                        () => HandleItems(), Cron.Hourly(recurrence.Interval));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Execution Time: [in an hour] at {DateTime.Now.Hour + 1}:{Minutes} minutes.\r\n");
                }
            case Documents.Vendors:
                {
                    string Minutes = recurrence.Interval.ToString();
                    Minutes = Minutes.Length != 2 ? "0" + recurrence.Interval : recurrence.Interval.ToString();

                    RecurringJob.AddOrUpdate($"Sync {recurrence.Document} in an hour at {DateTime.Now.Hour + 1}:{Minutes} minutes.",
                        () => SyncVendors(), Cron.Hourly(recurrence.Interval));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Execution Time: [in an hour] at {DateTime.Now.Hour + 1}:{Minutes} minutes.\r\n");
                }
            default:
                return BadRequest(
                    $"Please update payload with valid data types, update to 'document' to ('Departments' or 'Items' or 'Vendors') and 'Interval' to be between (0-59).");
        }
    }

    /// <summary>
    /// Returns Recurrence that will fires to Sync Departments Every Day Of Week.
    /// </summary>
    /// <param name="recurrence">'Document' Valid value is ('Departments' or 'Items' or 'Vendors'). - 'DayOfWeek' valid Values (Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday).</param>
    [HttpPost("Weekly")]
    public async Task<IActionResult> Weekly([FromBody] RecurrenceDailyDto recurrence)
    {
        if (recurrence.DayOfWeek
            is DayOfWeek.Saturday
            or DayOfWeek.Sunday
            or DayOfWeek.Monday
            or DayOfWeek.Tuesday
            or DayOfWeek.Thursday
            or DayOfWeek.Wednesday
            or DayOfWeek.Friday)
        {
            switch (recurrence.Document)
            {
                case Documents.Departments:
                    RecurringJob.AddOrUpdate($"Sync {recurrence.Document} Every {recurrence.DayOfWeek}.",
                        () => SyncDepartments(), Cron.Weekly(recurrence.DayOfWeek));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Scheduled Time: [Every {recurrence.DayOfWeek}]\r\n" +
                              $"Execution Time: [Next {recurrence.DayOfWeek}]\r\n");

                case Documents.Items:
                    RecurringJob.AddOrUpdate($"Sync {recurrence.Document} Every {recurrence.DayOfWeek}.",
                        () => SyncItems(),
                        Cron.Weekly(recurrence.DayOfWeek));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Scheduled Time: [Every {recurrence.DayOfWeek}]\r\n" +
                              $"Execution Time: [Next {recurrence.DayOfWeek}]\r\n");

                case Documents.Vendors:
                    RecurringJob.AddOrUpdate($"Sync {recurrence.Document} Every {recurrence.DayOfWeek} .",
                        () => SyncVendors(), Cron.Weekly(recurrence.DayOfWeek));

                    return Ok($"Task Name: [Sync {recurrence.Document}]\r\n" +
                              "Status: [Started]\r\n" +
                              $"Created Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" +
                              $"Scheduled Time: [Every {recurrence.DayOfWeek}]\r\n" +
                              $"Execution Time: [Next {recurrence.DayOfWeek}]\r\n");
            }
        }

        return BadRequest(
            $"Please update payload with valid data types, update to 'document' to ('Departments' or 'Items' or 'Vendors') and 'DayOfWeek' to any valid Values (Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday)..");
    }


    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task SyncDepartments()
    {
        var syncDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, Enums.UpdateType.SyncDepartment, out var needToSyncBasedOnSyncDate);
        var initialDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, Enums.UpdateType.SyncDepartment, out var needToSyncBasedOnInitialDate);

        var lastInitialDate = initialDate.ToSAPDateFormat();
        var lastSyncDate = syncDate.ToSAPDateFormat();

        var filter = $" WHERE (T0.[CreateDate] >= '{lastInitialDate}' OR T0.[UpdateDate] >= '{lastSyncDate}')";

        if (needToSyncBasedOnSyncDate || needToSyncBasedOnInitialDate)
        {
            await foreach (var requestResult in _departmentsHandler.SyncAsync(filter))
            {
                var result = requestResult.Message;
            }
        }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<string> SyncItems()
    {
        var itemsList = new List<ItemMasterData>();

        var resultSync = _itemsHandler.SyncAsync("");
        await foreach (var syncResult in resultSync)
        {

            foreach (var businessPartner in syncResult.EntityList)
            {
                itemsList.Add(businessPartner);

                //Console.WriteLine($"Business Partner: {businessPartner}");
            }
            //syncResult.UpdateResponse;

        }
        return JsonConvert.SerializeObject(itemsList);

    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task SyncVendors()
    {
        var initialDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, Enums.UpdateType.InitialVendors, out var needToSyncBasedOnInitialDate);
        var syncDate = SyncEntityService.CompareSyncDateWithDateNow(_unitOfWork, Enums.UpdateType.SyncVendors, out var needToSyncBasedOnSyncDate);

        var lastInitialDate = initialDate.ToSAPDateFormat();
        var lastSyncDate = syncDate.ToSAPDateFormat();

        var filter = $" AND (T0.[CreateDate] >= '{lastInitialDate}' OR T0.[UpdateDate] >= '{lastSyncDate}')";

        if (needToSyncBasedOnSyncDate || needToSyncBasedOnInitialDate)
        {
            var BpList = new List<BusinessPartner>();

            var resultSync = _vendorsHandler.SyncAsync(filter);
            await foreach (var syncResult in resultSync)
            {

                foreach (var businessPartner in syncResult.EntityList)
                {
                    BpList.Add(businessPartner);

                    //Console.WriteLine($"Business Partner: {businessPartner}");
                }
                //syncResult.UpdateResponse;

            }
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task HandleItems()
    {
        var filter = GetSyncQueryByRangOfDate();

        var LogMessage = "";
        await foreach (var syncResult in _itemsHandler.SyncAsync(filter))
        {
            if (syncResult.EntityList != null && syncResult.EntityList.Count > 0)
            {
                foreach (var itemMaster in syncResult.EntityList)
                {
                    LogMessage += syncResult.Message;
                }
            }
        }
    }
    private string GetSyncQueryByRangOfDate()
    {
        var dateFrom = DateTime.Now.AddYears(-5).ToSAPDateFormat();
        var dateTo = DateTime.Now.ToSAPDateFormat();

        return $" WHERE T0.[DocDate] Between '{dateFrom}' AND '{dateTo}' AND (T0.[U_SyncToPrism] IS NULL OR  T0.[U_SyncToPrism] = '' OR  T0.[U_SyncToPrism] = 'N')";
    }
}
