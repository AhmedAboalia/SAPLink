using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integration.Core.Models.System;

namespace Integration.EF.Data.Config
{
    internal class CredentialsConfiguration : IEntityTypeConfiguration<Credentials>
    {
        public void Configure(EntityTypeBuilder<Credentials> builder)
        {
            builder.HasKey(x => x.EnvironmentCode);
            builder.Property(x => x.EnvironmentCode).ValueGeneratedNever();

            //// builder.Property(x => x.CourseName).HasMaxLength(255); // nvarchar(255)

            //builder.Property(x => x.CourseName)
            //    .HasColumnType("VARCHAR")
            //    .HasMaxLength(255).IsRequired();

            //builder.Property(x => x.Price)
            //    .HasPrecision(15, 2);

            builder.ToTable("Credentials");
        }
    }
    
}
