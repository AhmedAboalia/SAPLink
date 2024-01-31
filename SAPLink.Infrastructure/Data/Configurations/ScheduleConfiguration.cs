using System;
using SAPLink.Domain.Common;
using SyncDocuments = SAPLink.Domain.Common.Schedules.SyncDocuments;
namespace SAPLink.Infrastructure.Data.Configurations;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedules>
{
    public void Configure(EntityTypeBuilder<Schedules> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(1, 1);

        builder.Property(s => s.DocumentName)
            .IsRequired()
            .HasMaxLength(255);  // or another appropriate limit


        // Configure the enum SyncDocuments as a column of type int.
        builder.Property(s => s.Document)
            .IsRequired()
            .HasConversion<int>();

        // Relationship configuration for the Recurring entity
        builder.HasMany(s => s.Times)
            .WithOne(r => r.Schedule)
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);  // Deleting a Schedule will delete associated Recurring entities



        builder.HasData(new Schedules[]
        {
            //new() { Id = 1, Document = SyncDocuments.Departments },
            //new() { Id = 2, Document = SyncDocuments.Vendors },
            new() { Id = 1, Document = SyncDocuments.Items },
            new() { Id = 2, Document = SyncDocuments.GoodsReceiptPos },
            //new() { Id = 5, Document = SyncDocuments.GoodsReceipts_Inbound},
            //new() { Id = 6, Document = SyncDocuments.GoodsIssues_Inbound},
            new() { Id = 3, Document = SyncDocuments.SalesInvoices},
            new() { Id = 4, Document = SyncDocuments.ReturnInvoices},
            //new() { Id = 9, Document = SyncDocuments.CustomerOrders},
            new() { Id = 5, Document = SyncDocuments.StockTransfers},
            //new() { Id = 11, Document = SyncDocuments.InventoryPosting},
            //new() { Id = 12, Document = SyncDocuments.GoodsReceipts_Outbound},
            //new() { Id = 13, Document = SyncDocuments.GoodsIssues_Outbound},
        });
    }

    internal class RecurringTimesConfiguration : IEntityTypeConfiguration<Times>
    {
        public void Configure(EntityTypeBuilder<Times> builder)
        {
            builder.ToTable("Times");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1);

            builder.HasData(
                new Times { Id = 1, Active = true, Time = new TimeOnly(7, 0), ScheduleId = (int)SyncDocuments.Items, TimeId = 1 },   // 7 AM
                new Times { Id = 2, Active = true, Time = new TimeOnly(12, 0), ScheduleId = (int)SyncDocuments.Items, TimeId = 2 },  // 12 PM
                new Times { Id = 3, Active = true, Time = new TimeOnly(17, 0), ScheduleId = (int)SyncDocuments.Items, TimeId = 3 },  // 5 PM

                new Times { Id = 4, Active = true, Time = new TimeOnly(7, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 1 },    // 7 AM
                new Times { Id = 5, Active = true, Time = new TimeOnly(12, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 2 },   // 12 PM
                new Times { Id = 6, Active = true, Time = new TimeOnly(17, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 3 },   // 5 PM



                new Times { Id = 7, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 1 }, // 1 PM
                new Times { Id = 8, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 2 }, // 6 PM
                new Times { Id = 9, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 3 },   // 12 AM 

                new Times { Id = 10, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 1 }, // 1 PM
                new Times { Id = 11, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 2 }, // 6 PM
                new Times { Id = 12, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 3 },  // 12 AM 

                new Times { Id = 13, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 1 }, // 1 PM
                new Times { Id = 14, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 2 }, // 6 PM
                new Times { Id = 15, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 3 }  // 12 AM 
            );
        }
    }
}