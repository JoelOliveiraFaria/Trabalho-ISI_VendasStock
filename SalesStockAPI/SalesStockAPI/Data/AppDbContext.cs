using Microsoft.EntityFrameworkCore;
using SalesAPI.Models;

namespace SalesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<WarehouseStock> WarehouseStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir chave composta para WarehouseStock
            modelBuilder.Entity<WarehouseStock>()
                .HasKey(ws => new { ws.WarehouseId, ws.Sku });

            base.OnModelCreating(modelBuilder);
        }
    }
}
