
using SAPLink.API.Services;
using SAPLink.Core.Models.System;
using SAPLink.Handler.SAP.Application;
using ServiceLayerHelper.RefranceModels;
using System.Drawing;

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
        sqliteOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        //sqliteOptions.MigrationsAssembly("SAPLink.API");

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

//builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

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

app.Run();
