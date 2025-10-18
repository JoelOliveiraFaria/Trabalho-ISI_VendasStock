using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesAPI.Models
{
    /// <summary>
    /// Representa stock de produto em armazém.
    /// </summary>
    [Table("warehouse_stock")]
    public class WarehouseStock
    {
        [Column("warehouse_id")]
        [MaxLength(50)]
        public string WarehouseId { get; set; } // ID do armazém

        [Column("sku")]
        [MaxLength(50)]
        public string Sku { get; set; } // Código do produto

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; } // Nome do produto

        [Column("quantity_available")]
        public int QuantityAvailable { get; set; } // Quantidade disponível

        [Column("minimum_level")]
        public int MinimumLevel { get; set; } // Nível mínimo

        [Column("last_updated")]
        [MaxLength(50)]
        public string LastUpdated { get; set; } // Data de atualização

        /// <summary>
        /// Verifica se stock está em ou abaixo do nível mínimo.
        /// </summary>
        [NotMapped]
        public bool IsLowStock => QuantityAvailable <= MinimumLevel;

        /// <summary>
        /// Estado atual do stock (Out of Stock, Low Stock, In Stock).
        /// </summary>
        [NotMapped]
        public string StockStatus => QuantityAvailable == 0 ? "Out of Stock" :
                                      IsLowStock ? "Low Stock" : "In Stock";
    }
}
