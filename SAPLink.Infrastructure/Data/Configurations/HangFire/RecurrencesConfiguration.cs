using SAPLink.Domain.System;

namespace SAPLink.Infrastructure.Data.Configurations.HangFire;

internal class RecurrencesConfiguration : IEntityTypeConfiguration<Recurrence>
{
    public void Configure(EntityTypeBuilder<Recurrence> builder)
    {
        builder.ToTable("Recurrences");

        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(1, 1);

        builder.HasData(new Recurrence[]
        {
            new() { Id = 1, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Departments },
            new() { Id = 2, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Vendors },
            new() { Id = 3, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.Items },
            new() { Id = 4, Recurring = Repeats.Hourly, Interval = 2, Document = Documents.GoodsReceiptPo },
        });
    }
}