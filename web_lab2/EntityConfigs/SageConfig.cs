using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web_lab2.Models;

namespace web_lab2.EntityConfigs
{
    internal class SageConfig : IEntityTypeConfiguration<Sage>
    {
        public void Configure(EntityTypeBuilder<Sage> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.HasMany<Book>(s => s.Books)
                .WithMany(b => b.Sages)
                .UsingEntity(j => j.ToTable("SageBook"));
        }
    }
}