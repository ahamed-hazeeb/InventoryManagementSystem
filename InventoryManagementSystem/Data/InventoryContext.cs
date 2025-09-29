using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
namespace InventoryManagementSystem.Data
{
    public class InventoryContext : DbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.InventoryItem)
                .WithMany()
                .HasForeignKey(oi => oi.InventoryItemId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.Supplier)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SupplierId);

            // Seed fake data for testing
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "TechCorp", ContactInfo = "tech@corp.com" },
                new Supplier { Id = 2, Name = "SupplyChain Inc.", ContactInfo = "supply@chain.com" }
            );

            modelBuilder.Entity<InventoryItem>().HasData(
                new InventoryItem { Id = 1, Name = "Laptop", Description = "High-end laptop", Quantity = 10, Price = 1000M, SupplierId = 1 },
                new InventoryItem { Id = 2, Name = "Monitor", Description = "24-inch monitor", Quantity = 15, Price = 300M, SupplierId = 2 }
            );

            // Add index for performance
            modelBuilder.Entity<InventoryItem>().HasIndex(i => i.Name);
        }
    }
}