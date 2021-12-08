using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web_lab2.Models;

namespace web_lab2.EntityConfigs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity(j => j.ToTable("UserRoles"));
        }
    }
}