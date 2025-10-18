using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataGenerator.Models
{
    [XmlType("stock_item")] // Elemento raiz XML: <stock_item>
    public class DirtyStock
    {
        [XmlElement("warehouse_id")]
        public string warehouse_id { get; set; } // ID do armazém

        [XmlElement("sku")]
        public string sku { get; set; } // Código do produto

        [XmlElement("product_name")]
        public string product_name { get; set; } // Nome do produto

        [XmlElement("quantity_available")]
        public string quantity_available { get; set; } // Quantidade disponível

        [XmlElement("minimum_level")]
        public string minimum_level { get; set; } // Nível mínimo de stock

        [XmlElement("location")]
        public string location { get; set; } // Localização no armazém

        [XmlElement("last_updated")]
        public string last_updated { get; set; } // Data de última atualização

        [XmlElement("supplier")]
        public string supplier { get; set; } // Fornecedor do produto
    }
}
