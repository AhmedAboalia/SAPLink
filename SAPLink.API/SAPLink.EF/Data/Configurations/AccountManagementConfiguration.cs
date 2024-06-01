using System;

namespace SAPLink.EF.Data.Configurations;

internal class AccountManagementConfiguration : IEntityTypeConfiguration<AccountManagement>
{

    public void Configure(EntityTypeBuilder<AccountManagement> builder)
    {
        builder.ToTable("Accounts");

        //builder.Property(e => e.Id)
        //    .ValueGeneratedOnAdd()
        //    .UseIdentityColumn(1, 1);


        builder.HasData(new AccountManagement[]
        {
            new() { Id = 1, PaymentTypeCode = PaymentTypes.Tamara, PaymentTypeName = "Tamara", Account = "11180050100"},
            new() { Id= 2, PaymentTypeCode = PaymentTypes.Emkan, PaymentTypeName = "Emkan", Account = "11180050100"},
            new() { Id= 3,PaymentTypeCode = PaymentTypes.Tabby, PaymentTypeName = "Tabby", Account = "11180070100"},
        });
    }
}