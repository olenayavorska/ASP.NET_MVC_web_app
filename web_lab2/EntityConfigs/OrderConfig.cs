using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web_lab2.Models;

namespace web_lab2.EntityConfigs
{
    internal class OrderConfig: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            
            builder.HasMany(o => o.Books)
                .WithMany(b => b.Orders)
                .UsingEntity<OrdersBooks>(
                    j => j.HasOne(ob => ob.Book)
                        .WithMany(o => o.OrdersDetails)
                        .HasForeignKey(ob => ob.BookId),
                    j => j.HasOne(ob => ob.Order)
                        .WithMany(b => b.OrdersDetails)
                        .HasForeignKey(ob => ob.OrderId),
                    j =>
                    {
                        j.HasKey(ob => new { ob.OrderId, ob.BookId });
                        j.ToTable("OrdersBooks");
                    }
                );

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId);
        }
    }
}