
using SAPLink.API.Services;
using ServiceLayerHelper.RefranceModels;
using System.Drawing;
using SAPLink.API.Controllers;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Departments;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Inventory;
using SAPLink.Application.Prism.Handlers.InboundData.Merchandise.Vendors;
using SAPLink.Application.SAP.Handlers;
using SAPLink.Domain;
using SAPLink.Domain.Connection;
using SAPLink.Infrastructure;
using SAPLink.Infrastructure.Data;
using SAPLink.Infrastructure.Interfaces;
using SAPLink.Domain.System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new() { Title = "SAPLink (SAP Business One & Prism Integration) API", Version = "v2" });

    //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var commentFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    //c.IncludeXmlComments(commentFilePath);
});

#region Old Manual Connection Strings

//var connectionString = builder.Configuration.GetConnectionString("PrismLiveConnection");
//var connectionString = builder.Configuration.GetConnectionString("PrismTestConnection");
//var connectionString = builder.Configuration.GetConnectionString("LocalConnection");

//var connectionString = builder.Configuration.GetConnectionString("SAPLiveConnection");

#endregion

#region SQL Lite
//var connectionString = "Data Source=Database\\HangFire.db;";

var connectionString = ConnectionStringFactory.SqlLite();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        //sqliteOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        sqliteOptions.MigrationsAssembly("SAPLink.EF");

        //sqliteOptions.UseSqlCipher("<encryption_key>");
    }));


builder.Services.AddHangfire(x => x.UseSQLiteStorage(connectionString));
builder.Services.AddHangfireServer();

#endregion

#region SQL Server

//var connectionString = ConnectionStringFactory.SqlServer();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString, sqlOptions  => { sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName); }));

//builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));

#endregion

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddScoped<RecurrenceService>();

// Register your DbContext and UnitOfWork
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<UnitOfWork>();

// Register other services
builder.Services.AddScoped<Clients>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<DepartmentsHandler>();
builder.Services.AddScoped<VendorsService>();
builder.Services.AddScoped<VendorsHandler>();
builder.Services.AddScoped<ServiceLayerHandler>();
builder.Services.AddScoped<ItemsService>();
builder.Services.AddScoped<ItemsHandler>();

// Register your controller
builder.Services.AddScoped<ScheduleSyncs>();

//builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));


builder.Services.AddScoped(provider =>
{
    // Get required dependencies
    var unitOfWork = provider.GetRequiredService<UnitOfWork>();
    var includes = new string[] { "Credentials", "Credentials.Subsidiaries" };

    // Fetch data and create the client
    var clients = unitOfWork.Clients.GetAll(includes).ToList();
    var localClient = clients.Find(x => x.Id == (int)Enums.Environments.Local);

    // Return the configured client
    return localClient;
});

builder.Services.AddControllers().AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "SAPLink (SAP Business One & Prism) Synchronization API v1");
    //options.RoutePrefix = string.Empty;
    //options.RoutePrefix = "api";
});


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
//    {
//        Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
//    });
//});

//app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = "https://localhost:7208/dashboard" });
//app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = "https://localhost:7208/swagger/index.html" });


//app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = $"https://localhost:7208/swagger/index.html" });


//app.UseHangfireServer(new BackgroundJobServerOptions
//{
//    HeartbeatInterval = new TimeSpan(0, 1, 0),
//    ServerCheckInterval = new System.TimeSpan(0, 1, 0),
//    SchedulePollingInterval = new System.TimeSpan(0, 1, 0)
//});

app.UseHangfireServer(new BackgroundJobServerOptions
{
    WorkerCount = 1
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
