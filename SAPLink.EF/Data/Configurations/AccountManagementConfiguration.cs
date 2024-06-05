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
            new() { Id = 1, PaymentType = 2, PaymentTypeCode = PaymentTypes.Tamara, PaymentTypeName = "Tamara", Account = "11180050100", UsePerStoreAccount = false, Account1 = "", StoreCode = "",Account2 = ""},
            new() { Id= 2, PaymentType = 2, PaymentTypeCode = PaymentTypes.Emkan, PaymentTypeName = "Emkan", Account = "11180050100", UsePerStoreAccount = false, Account1 = "", StoreCode = "",Account2 = ""},
            new() { Id= 3, PaymentType = 2, PaymentTypeCode = PaymentTypes.Tabby, PaymentTypeName = "Tabby", Account = "11180070100", UsePerStoreAccount = false, Account1 = "", StoreCode = "",Account2 = ""},

            new() { Id= 4, PaymentType = 2, PaymentTypeCode = PaymentTypes.MasterCard, PaymentTypeName = "Master Card", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},
            new() { Id= 5, PaymentType = 2, PaymentTypeCode = PaymentTypes.Mada, PaymentTypeName = "Mada", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},
            new() { Id= 6, PaymentType = 2, PaymentTypeCode = PaymentTypes.Return, PaymentTypeName = "Return", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},

            new() { Id= 7, PaymentType = 2, PaymentTypeCode = PaymentTypes.Visa, PaymentTypeName = "Visa", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},
            new() { Id= 8, PaymentType = 2, PaymentTypeCode = PaymentTypes.AmericanExpress, PaymentTypeName = "American Express", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},

            new() { Id= 9, PaymentType = 0, PaymentTypeCode = PaymentTypes.Cash, PaymentTypeName = "Cash", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},
            new() { Id= 10, PaymentType = 3, PaymentTypeCode = PaymentTypes.Deposit, PaymentTypeName = "Deposit", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},
            new() { Id= 11, PaymentType = 7, PaymentTypeCode = PaymentTypes.BankTransfer, PaymentTypeName = "Bank Transfer", Account = "", UsePerStoreAccount = true, Account1 = "1101", StoreCode = "",Account2 = "0100"},

        });
    }
}