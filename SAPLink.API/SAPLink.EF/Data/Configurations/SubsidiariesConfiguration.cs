namespace SAPLink.EF.Data.Configurations;

internal class SubsidiariesConfiguration : IEntityTypeConfiguration<Subsidiaries>
{
    public void Configure(EntityTypeBuilder<Subsidiaries> builder)
    {
        builder.ToTable("Subsidiaries");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired();

        builder.HasOne(s => s.Credential)
            .WithMany(cr => cr.Subsidiaries)
            .HasForeignKey(s => s.CredentialId)
            .IsRequired();

        builder.HasData(SubsidiariesList.GetSubsidiaries());
    }
}
internal static class SubsidiariesList
{
    public static List<Subsidiaries>? GetSubsidiaries()
    {
        return new List<Subsidiaries>
        {
            new()
            {
                Id = (int)Environments.Production,
                CredentialId = (int)Environments.Production, // Foreign key referencing the credential
                Name = "AlKaffary - (Production)",

                Number = 1,
                SID = 664651285000113257,
                Clerksid = "674955099100039866",
                ActivePriceLevelid = "664651377000135721",
                ActiveSeasonSid = "664651377000169734",
                ActiveStoreSid = "664651285000116261",
                ActiveTaxCode = "664651377000183746",
            },
            new()
            {
                Id = (int)Environments.Test,
                CredentialId = (int)Environments.Test, // Foreign key referencing the credential
                Name = "AlKaffary - (Test)",

                Number = 1,
                SID = 663852103000153257,
                Clerksid = "674654182000171601",
                ActivePriceLevelid = "663852140000113721",
                ActiveSeasonSid = "663852140000143734",
                ActiveStoreSid = "674650601000132347",
                ActiveTaxCode = "663852140000157746",
            },
            //new()
            //{
            //    Id = (int)Environments.Test,
            //    CredentialId = (int)Environments.Test, // Foreign key referencing the credential
            //    Name = "Kaffary - (New Test)",

            //    Number = 3,
            //    SID = 674650600000126277,
            //    Clerksid = "674654182000171601",
            //    ActivePriceLevelid = "674650602000169420",
            //    ActiveSeasonSid = "663852140000143734",
            //    ActiveStoreSid = "674650601000132347",
            //    ActiveTaxCode = "674650601000152353",
            //},
            new()
            {
                Id = (int)Environments.Local,
                CredentialId = (int)Environments.Local, // Foreign key referencing the credential
                Name = "Local Environment (Public API)",

                Number = 1,
                SID = 675951888000146257,
                Clerksid = "675951888000149260",
                ActivePriceLevelid = "675951940000193772",
                ActiveSeasonSid = "675951941000138785",
                ActiveStoreSid = "675951888000150261",
                ActiveTaxCode = "",
            },
        };
    }
}