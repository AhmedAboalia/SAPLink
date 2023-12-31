﻿namespace SAPLink.EF.Data.Configurations;

public class ItemsLogConfiguration : IEntityTypeConfiguration<Logger<ItemMasterData>>
{
    public void Configure(EntityTypeBuilder<Logger<ItemMasterData>> builder)
    {
        builder.ToTable("ItemsLogger");
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id).IsRequired();

        //builder.HasMany(c => c.Credentials)
        //    .WithOne(cr => cr.Client)
        //    .HasForeignKey(cr => cr.ClientId)
        //    .IsRequired();
    }
}