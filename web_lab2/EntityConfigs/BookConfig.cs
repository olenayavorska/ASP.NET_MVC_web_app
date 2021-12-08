using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web_lab2.Models;

namespace web_lab2.EntityConfigs
{
    internal class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.HasMany(b => b.Sages)
                .WithMany(s => s.Books)
                .UsingEntity(j => j.ToTable("SageBook"));

            builder.HasMany(b => b.Orders)
                .WithMany(o => o.Books)
                .UsingEntity<OrdersBooks>(
                    j => j.HasOne(ob => ob.Order)
                        .WithMany(b => b.OrdersDetails)
                        .HasForeignKey(ob => ob.OrderId),
                    j => j.HasOne(ob => ob.Book)
                        .WithMany(o => o.OrdersDetails)
                        .HasForeignKey(ob => ob.BookId),
                    j =>
                    {
                        j.HasKey(ob => new {ob.OrderId, ob.BookId});
                        j.ToTable("OrdersBooks");
                    }
                );
        }
    }
}