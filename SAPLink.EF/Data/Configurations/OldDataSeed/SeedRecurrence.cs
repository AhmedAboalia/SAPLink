namespace SAPLink.EF.Data.Configurations.OldDataSeed
{
    internal class SeedRecurrence
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recurrence>().HasData(new Recurrence[]
            {
                new() { Id = (int)Documents.Departments, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Departments },
                new() { Id = (int)Documents.Vendors, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Vendors },
                new() { Id = (int)Documents.Items, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Items },
                new() { Id = (int)Documents.GoodsReceiptPo, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.GoodsReceiptPo },
            });
        }


    }
}
