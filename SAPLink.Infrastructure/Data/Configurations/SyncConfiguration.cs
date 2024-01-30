using System;
using SAPLink.Domain.Models.System;

namespace SAPLink.Infrastructure.Data.Configurations;

internal class SyncConfiguration : IEntityTypeConfiguration<Sync>
{
    public void Configure(EntityTypeBuilder<Sync> builder)
    {
        builder.ToTable("Sync");

        //builder.Property(e => e.Id)
        //    .ValueGeneratedOnAdd()
        //    .UseIdentityColumn(1, 1);


        builder.HasData(new Sync[]
        {
            new() { UpdateType = UpdateType.InitialDepartment, Name = "InitialDepartment", Date = new DateTime(2023, 6, 15) },
            new() { UpdateType = UpdateType.SyncDepartment, Name = "SyncDepartment", Date = new DateTime(2023, 6, 15) },

            new() { UpdateType = UpdateType.InitialVendors, Name = "InitialVendors", Date = new DateTime(2023, 6, 15) },
            new() { UpdateType = UpdateType.SyncVendors, Name = "SyncVendors", Date = new DateTime(2023, 6, 15) },

            new() { UpdateType = UpdateType.InitialItems, Name = "InitialItems", Date = new DateTime(2023, 6, 15) },
            new() { UpdateType = UpdateType.SyncItems, Name = "SyncItems", Date = new DateTime(2023, 6, 15) },

            new()
            {
                UpdateType = UpdateType.InitialGoodsReceiptPO, Name = "InitialGoodsReceiptPO",
                Date = new DateTime(2023, 6, 15)
            },
            new()
            {
                UpdateType = UpdateType.SyncGoodsReceiptPO, Name = "SyncGoodsReceiptPO", Date = new DateTime(2023, 6, 15)
            }
        });
    }
}