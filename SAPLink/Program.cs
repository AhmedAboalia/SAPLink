using SAPLink.Core.Connection;
using SAPLink.EF.Data;
using SAPLink.Forms;
using SAPLink.Handler.Prism.Handlers;
using SAPLink.Handler.Prism.Handlers.InboundData.Merchandise.Vendors;
using Serilog;
using Serilog.Core;
using System.Diagnostics;

namespace SAPLink;

internal static class Program
{
    private static Clients Client { get; set; }
    private static readonly ApplicationDbContext Context = new();
    private static readonly UnitOfWork UnitOfWork = new(Context);
    private static ServiceLayerHandler _serviceLayer;
    private static DepartmentService _departmentService;
    private static VendorsService _vendorsHandler;
    private static ItemsService _itemsService;

    public static bool IsSapConnected = false;
    public static bool IsPrismConnected = false;

    private static ILogger _loger;


    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        _loger = Handler.Prism.Handlers.Helper.CreateLoggerConfiguration("SAP Link", "General Logs");

        MigrateDatabase();
        _loger.Information("Successfully Update Database.");

        GetActiveClient();
        _loger.Information("Successfully Loaded Get Active Client.");

        _serviceLayer = new ServiceLayerHandler(Client);
        _loger.Information("Successfully Initiate SAP Business One Service Layer.");

        _departmentService = new DepartmentService(Client);
        _loger.Information("Successfully Initiate Department Service.");

        _vendorsHandler = new VendorsService(Client);
        _loger.Information("Successfully Initiate Vendors Service.");

        _itemsService = new ItemsService(Client, _departmentService, _vendorsHandler);
        _loger.Information("Successfully Initiate Items Service.");

        Application.ThreadException += Application_ThreadException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        Application.Run(new Login(UnitOfWork, _serviceLayer, _departmentService, _itemsService, Client));
        Log.CloseAndFlush();

    }

    private static void GetActiveClient()
    {
        var activeConnection = ConnectionStringFactory.GetActiveConnection();

        string[] includes = { "Credentials", "Credentials.Subsidiaries" };
        Client = UnitOfWork.Clients.FindAsync(c => c.Id == activeConnection.Id, includes).Result;
        Client.Active = true;
        UnitOfWork.Clients.Update(Client);
    }

    private static void MigrateDatabase()
    {
#if DEBUG
        //Context.Database.EnsureDeleted();
#endif
        //var serverInfo = ConnectionStringFactory.ReadConnections();
        //if (serverInfo == null || !serverInfo.Any())
        //    ConnectionStringFactory.WriteConnectionsToFile();

        if (!Context.Database.EnsureCreated())
        {
            Context.Database.MigrateAsync();
        }
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        _loger.Fatal(e.ExceptionObject as Exception, "An unhandled exception occurred.");
    }

    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        _loger.Fatal(e.Exception, "An unhandled thread exception occurred.");
    }
}