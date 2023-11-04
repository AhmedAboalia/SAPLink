﻿using System;
using SyncDocuments = SAPLink.Core.Models.Schedule.SyncDocuments;
namespace SAPLink.EF.Data.Configurations;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
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
        builder.HasMany(s => s.Recurring)
            .WithOne(r => r.Schedule)
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);  // Deleting a Schedule will delete associated Recurring entities

      

        builder.HasData(new Schedule[]
        {
            new() { Id = 1, Document = SyncDocuments.Departments },
            new() { Id = 2, Document = SyncDocuments.Vendors },
            new() { Id = 3, Document = SyncDocuments.Items },
            new() { Id = 4, Document = SyncDocuments.GoodsReceiptPos },
            new() { Id = 5, Document = SyncDocuments.GoodsReceipts_Inbound},
            new() { Id = 6, Document = SyncDocuments.GoodsIssues_Inbound},
            new() { Id = 7, Document = SyncDocuments.SalesInvoices},
            new() { Id = 8, Document = SyncDocuments.ReturnInvoices},
            new() { Id = 9, Document = SyncDocuments.CustomerOrders},
            new() { Id = 10, Document = SyncDocuments.StockTransfers},
            new() { Id = 11, Document = SyncDocuments.InventoryPosting},
            new() { Id = 12, Document = SyncDocuments.GoodsReceipts_Outbound},
            new() { Id = 13, Document = SyncDocuments.GoodsIssues_Outbound},
        });
    }

    internal class RecurringConfiguration : IEntityTypeConfiguration<Recurring>
    {
        public void Configure(EntityTypeBuilder<Recurring> builder)
        {
            builder.ToTable("Recurrings");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1);

            builder.HasData(
                new Recurring { Id = 1, Active = true, Time = new TimeOnly(7, 0), ScheduleId = (int)SyncDocuments.Items, TimeId = 1},   // 7 AM
                new Recurring { Id = 2, Active = true, Time = new TimeOnly(12, 0), ScheduleId = (int)SyncDocuments.Items, TimeId = 2 },  // 12 PM
                new Recurring { Id = 3, Active = true, Time = new TimeOnly(17, 0), ScheduleId = (int)SyncDocuments.Items , TimeId = 3 },  // 5 PM

                new Recurring { Id = 4, Active = true, Time = new TimeOnly(7, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 1 },    // 7 AM
                new Recurring { Id = 5, Active = true, Time = new TimeOnly(12, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 2 },   // 12 PM
                new Recurring { Id = 6, Active = true, Time = new TimeOnly(17, 0), ScheduleId = (int)SyncDocuments.GoodsReceiptPos, TimeId = 3 },   // 5 PM



                new Recurring { Id = 7, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 1 }, // 1 PM
                new Recurring { Id = 8, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 2 }, // 6 PM
                new Recurring { Id = 9, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.SalesInvoices, TimeId = 3 },   // 12 AM 

                new Recurring { Id = 10, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 1 }, // 1 PM
                new Recurring { Id = 11, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 2 }, // 6 PM
                new Recurring { Id = 12, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.ReturnInvoices, TimeId = 3 },  // 12 AM 

                new Recurring { Id = 13, Active = true, Time = new TimeOnly(13, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 1 }, // 1 PM
                new Recurring { Id = 14, Active = true, Time = new TimeOnly(18, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 2 }, // 6 PM
                new Recurring { Id = 15, Active = true, Time = new TimeOnly(0, 0), ScheduleId = (int)SyncDocuments.StockTransfers, TimeId = 3 }  // 12 AM 
            );
        }
    }
}