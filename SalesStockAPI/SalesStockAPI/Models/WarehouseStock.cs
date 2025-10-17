using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesAPI.Models
{
    [Table("warehouse_stock")]
    public class WarehouseStock
    {
        [Column("warehouse_id")]
        [MaxLength(50)]
        public string WarehouseId { get; set; }

        [Column("sku")]
        [MaxLength(50)]
        public string Sku { get; set; }

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [Column("quantity_available")]
        public int QuantityAvailable { get; set; }

        [Column("minimum_level")]
        public int MinimumLevel { get; set; }

        [Column("last_updated")]
        [MaxLength(50)]
        public string LastUpdated { get; set; }  

        // Propriedades calculadas
        [NotMapped]
        public bool IsLowStock => QuantityAvailable <= MinimumLevel;

        [NotMapped]
        public string StockStatus => QuantityAvailable == 0 ? "Out of Stock" :
                                      IsLowStock ? "Low Stock" : "In Stock";
    }
}
