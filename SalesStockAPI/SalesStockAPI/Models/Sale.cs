using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesAPI.Models
{
    [Table("sales")]
    public class Sale
    {
        [Column("sale_id")]
        [MaxLength(50)]
        public string SaleId { get; set; }

        [Column("sale_date")]
        public string SaleDate { get; set; }

        [Column("customer_id")]
        [MaxLength(50)]
        public string CustomerId { get; set; }

        [Column("customer_email")]
        [MaxLength(100)]
        public string CustomerEmail { get; set; }

        [Column("sku")]
        [MaxLength(50)]
        public string Sku { get; set; }

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price")]
        [MaxLength(200)]
        public string UnitPrice { get; set; }

        [Column("total_amount")]
        [MaxLength(200)]
        public string TotalAmount { get; set; }

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; }

        [NotMapped]
        public decimal UnitPriceDecimal =>
            decimal.TryParse(UnitPrice?.Replace(",", "."), out var result) ? result : 0;

        [NotMapped]
        public decimal TotalAmountDecimal =>
            decimal.TryParse(TotalAmount?.Replace(",", "."), out var result) ? result : 0;
    }
}
