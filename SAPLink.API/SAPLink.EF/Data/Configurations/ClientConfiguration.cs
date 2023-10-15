namespace SAPLink.EF.Data.Configurations;

public class ClientsConfiguration : IEntityTypeConfiguration<Clients>
{
    public void Configure(EntityTypeBuilder<Clients> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(x => x.Id);
        builder.Property(c => c.Id).IsRequired();

        builder.HasMany(c => c.Credentials)
            .WithOne(cr => cr.Client)
            .HasForeignKey(cr => cr.ClientId)
            .IsRequired();

        builder.HasData(ClientsList.GetClients());
    }
}

internal static class ClientsList
{
    public static List<Clients>? GetClients()
    {
        return new List<Clients>
        {
            new()
            {
                Id = (int)Environments.Production,
                Name = "Al-Kaffary Subsidiary - SAP Live DB (KaffaryDB)",//Live Env. - 
                Active = false,
            },
            new()
            {
                Id = (int)Environments.Test,
                Name = "Test Subsidiary - SAP Test DB (TESTDB)",//Test Env. - 
                Active = false,
            },
            //new()
            //{
            //    Id = (int)Environments.Local,
            //    Name = "Fakeeh Vision Subsidiary - SAP Local DB (SBODemoGB)",//Local Env. - 
            //    Active = true,
            //},
        };
    }
}