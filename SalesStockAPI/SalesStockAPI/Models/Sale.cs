using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesAPI.Models
{
    /// <summary>
    /// Representa uma venda no sistema.
    /// </summary>
    [Table("sales")]
    public class Sale
    {
        [Column("sale_id")]
        [MaxLength(50)]
        public string SaleId { get; set; } // ID da venda

        [Column("sale_date")]
        public string SaleDate { get; set; } // Data da venda

        [Column("customer_id")]
        [MaxLength(50)]
        public string CustomerId { get; set; } // ID do cliente

        [Column("customer_email")]
        [MaxLength(100)]
        public string CustomerEmail { get; set; } // Email do cliente

        [Column("sku")]
        [MaxLength(50)]
        public string Sku { get; set; } // Código do produto

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; } // Nome do produto

        [Column("quantity")]
        public int Quantity { get; set; } // Quantidade vendida

        [Column("unit_price")]
        [MaxLength(200)]
        public string UnitPrice { get; set; } // Preço unitário

        [Column("total_amount")]
        [MaxLength(200)]
        public string TotalAmount { get; set; } // Valor total

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } // Estado da venda

        /// <summary>
        /// Converte UnitPrice para decimal.
        /// </summary>
        [NotMapped]
        public decimal UnitPriceDecimal =>
            decimal.TryParse(UnitPrice?.Replace(",", "."), out var result) ? result : 0;

        /// <summary>
        /// Converte TotalAmount para decimal.
        /// </summary>
        [NotMapped]
        public decimal TotalAmountDecimal =>
            decimal.TryParse(TotalAmount?.Replace(",", "."), out var result) ? result : 0;
    }
}
