using Microsoft.EntityFrameworkCore;
using SalesAPI.Models;

namespace SalesAPI.Data
{
    /// <summary>
    /// Contexto de base de dados da aplicação SalesAPI.
    /// Gere a conexão com a base de dados e define os DbSets para as entidades.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância do contexto AppDbContext.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// DbSet para a entidade Sales (Vendas).
        /// Representa a tabela de vendas na base de dados.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// DbSet para a entidade WarehouseStock (Stock de Armazém).
        /// Representa a tabela de inventário de armazém na base de dados.
        /// </summary>
        public DbSet<WarehouseStock> WarehouseStock { get; set; }

        /// <summary>
        /// Configura o modelo de dados usando Fluent API.
        /// Define chaves primárias, relações e restrições das entidades.
        /// </summary>
        /// <param name="modelBuilder">Builder para configuração do modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define chave primária composta para WarehouseStock (WarehouseId + Sku)
            // Garante que a combinação de armazém e produto seja única
            modelBuilder.Entity<WarehouseStock>()
                .HasKey(ws => new { ws.WarehouseId, ws.Sku });

            base.OnModelCreating(modelBuilder);
        }
    }
}
