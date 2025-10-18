using System;
using System.Xml.Serialization;

namespace DataGenerator.Models
{
    [XmlType("sale")] // Elemento raiz XML: <sale>
    public class DirtySells
    {
        [XmlElement("sale_id")]
        public string sale_id { get; set; } // ID da venda

        [XmlElement("sale_date")]
        public string sale_date { get; set; } // Data da venda

        [XmlElement("customer_id")]
        public string customer_id { get; set; } // ID do cliente

        [XmlElement("customer_email")]
        public string customer_email { get; set; } // Email do cliente

        [XmlElement("sku")]
        public string sku { get; set; } // Código do produto vendido

        [XmlElement("product_name")]
        public string product_name { get; set; } // Nome do produto

        [XmlElement("quantity")]
        public string quantity { get; set; } // Quantidade vendida

        [XmlElement("unit_price")]
        public string unit_price { get; set; } // Preço unitário

        [XmlElement("total_amount")]
        public string total_amount { get; set; } // Valor total

        [XmlElement("status")]
        public string status { get; set; } // Estado da venda
    }
}
