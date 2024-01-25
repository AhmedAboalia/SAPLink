using SAPLink.Core.Connection;
using SAPLink.EF.Data.Configurations;
using SAPLink.EF.Data.Configurations.HangFire;
using static SAPLink.EF.Data.Configurations.ScheduleConfiguration;

namespace SAPLink.EF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Recurrence> Recurrences { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        //public DbSet<Logger<ItemMasterData>> ItemsLog { get; set; }
        //public DbSet<Logger<ItemGroups>> ItemGroupsLog { get; set; }
        //public DbSet<Logger<Vendor>> VendorsLog { get; set; }
        //public DbSet<Logger<GoodsReceiptPO>> GoodsReceiptPosLog { get; set; }
        //public DbSet<Logger<GoodsReceipt>> GoodsReceiptLog { get; set; }
        //public DbSet<Logger<GoodsIssue>> GoodsIssueLog { get; set; }

        //public DbSet<Subsidiary> Subsidiary { get; set; }
        //public DbSet<Season> Season { get; set; }
        //public DbSet<PriceLevel> PriceLevel { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            var connectionString = ConnectionStringFactory.SqlLite();

            if (!optionsBuilder.IsConfigured)
            {
                #region Old Manual Connection Strings

                //var PrismLiveConnection = "Server=KAFFARYRETAIL;Database=INTEGRATION-DATABASE;TrustServerCertificate=True;User Id=sa;Password=RetailTec@123;";
                //var PrismTestConnection = "Server=POSTest;Database=INTEGRATION-DATABASE;TrustServerCertificate=True;User Id=sa;Password=RetailTec@123;";
                //var LocalConnection = "Server=ABOALIA;Database=INTEGRATION-DATABASE;TrustServerCertificate=True;User Id=sa;Password=P@ssw0rd;";// Trusted_Connection=True;
                //var LocalConnection = "Server=Aboalia;Database=SBODemoGB;TrustServerCertificate=True;User Id=sa;Password=P@ssw0rd;";// Trusted_Connection=True;
                //var SAPLiveConnection = "Server=SAP-TEST;Database=INTEGRATION-DATABASE;TrustServerCertificate=True;User Id=sa;Password=sap123456*;";

                //var connectionString = PrismLiveConnection;
                //var connectionString = PrismTestConnection;

                //var connectionString = LocalConnection;

                //var connectionString = SAPLiveConnection;
                #endregion

                #region SQL Lite

                optionsBuilder.UseSqlite(connectionString, sqliteOptions =>
                {
                    sqliteOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    //sqliteOptions.UseSqlCipher("<encryption_key>");
                });

                #endregion

                #region SQL Server

                //var connectionString = ConnectionStringFactory.SqlServer();

                //optionsBuilder.UseSqlServer(connectionString, 
                //    sqlOptions => { sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName); });

                #endregion
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClientsConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialsConfiguration());
            modelBuilder.ApplyConfiguration(new SubsidiariesConfiguration());

            modelBuilder.ApplyConfiguration(new RecurrencesConfiguration());
            modelBuilder.ApplyConfiguration(new SyncConfiguration());

            //modelBuilder.ApplyConfiguration(new ItemsLogConfiguration());

            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringTimesConfiguration());

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


            //SeedCredentials.Seed(modelBuilder);
            //SeedRecurrence.Seed(modelBuilder);
            //SeedSync.Seed(modelBuilder);

            //SeedSubsidiary.Seed(modelBuilder);
        }
    }
}