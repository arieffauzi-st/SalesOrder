using Microsoft.EntityFrameworkCore;
using SalesOrder.Entities;

namespace SalesOrder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ComCustomer> ComCustomers { get; set; } = null!;
        public DbSet<SoOrder> SoOrders { get; set; } = null!;
        public DbSet<SoItem> SoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComCustomer>(entity =>
            {
                entity.ToTable("COM_CUSTOMER");
                entity.HasKey(e => e.ComCustomerId);
                entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID");
                entity.Property(e => e.CustomerName).HasColumnName("CUSTOMER_NAME").HasMaxLength(100);
            });

            modelBuilder.Entity<SoOrder>(entity =>
            {
                entity.ToTable("SO_ORDER");
                entity.HasKey(e => e.SoOrderId);
                entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID");
                entity.Property(e => e.OrderNo).HasColumnName("ORDER_NO").HasMaxLength(20);
                entity.Property(e => e.OrderDate).HasColumnName("ORDER_DATE").HasDefaultValueSql("'1900-01-01'");
                entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID").HasDefaultValue(-99);
                entity.Property(e => e.Address).HasColumnName("ADDRESS").HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ComCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SoItem>(entity =>
            {
                entity.ToTable("SO_ITEM");
                entity.HasKey(e => e.SoItemId);
                entity.Property(e => e.SoItemId).HasColumnName("SO_ITEM_ID");
                entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID").HasDefaultValue(-99);
                entity.Property(e => e.ItemName).HasColumnName("ITEM_NAME").HasMaxLength(100);
                entity.Property(e => e.Quantity).HasColumnName("QUANTITY").HasDefaultValue(-99);
                entity.Property(e => e.Price).HasColumnName("PRICE").HasDefaultValue(0.0);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.SoOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}