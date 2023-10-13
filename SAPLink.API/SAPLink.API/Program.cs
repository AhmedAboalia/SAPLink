var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SAPLink (SAP Business One & Prism Integration) API", Version = "v1" });

    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var commentFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    c.IncludeXmlComments(commentFilePath);
});

#region Old Manual Connection Strings

//var PrismLiveConnection = builder.Configuration.GetConnectionString("PrismLiveConnection");
//var PrismTestConnection = builder.Configuration.GetConnectionString("PrismTestConnection");
//var LocalConnection = builder.Configuration.GetConnectionString("LocalConnection");

//var SAPLiveConnection = builder.Configuration.GetConnectionString("SAPLiveConnection");

//var connectionString = PrismLiveConnection;
//var connectionString = PrismTestConnection;

//var connectionString = LocalConnection;

//var connectionString = SAPLiveConnection;

#endregion


#region SQL Lite

var connectionString = ConnectionStringFactory.SqlLite();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        sqliteOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        //sqliteOptions.UseSqlCipher("<encryption_key>");
    }));

builder.Services.AddHangfire(x => x.UseSQLiteStorage(connectionString));

#endregion


#region SQL Server

//var connectionString = ConnectionStringFactory.SqlServer();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString, sqlOptions  => { sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName); }));

//builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));

#endregion


builder.Services.AddHangfireServer();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAPLink (SAP Business One & Prism Integration) API v1"));
}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
//    {
//        Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
//    });
//});
//app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = "https://localhost:7208/dashboard" });
//app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = "https://localhost:7208/swagger/index.html" });


app.UseHangfireDashboard("/dashboard", new DashboardOptions { AppPath = $"https://localhost:7208/swagger/index.html" });


//app.UseHangfireServer(new BackgroundJobServerOptions
//{
//    HeartbeatInterval = new TimeSpan(0, 1, 0),
//    ServerCheckInterval = new System.TimeSpan(0, 1, 0),
//    SchedulePollingInterval = new System.TimeSpan(0, 1, 0)
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
