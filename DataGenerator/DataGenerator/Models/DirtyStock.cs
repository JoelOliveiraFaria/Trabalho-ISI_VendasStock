using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataGenerator.Models
{
    [XmlType("stock_item")]
    public class DirtyStock
    {
        [XmlElement("warehouse_id")]
        public string warehouse_id { get; set; }

        [XmlElement("sku")]
        public string sku { get; set; }

        [XmlElement("product_name")]
        public string product_name { get; set; }

        [XmlElement("quantity_available")]
        public string quantity_available { get; set; }

        [XmlElement("minimum_level")]
        public string minimum_level { get; set; }

        [XmlElement("location")]
        public string location { get; set; }

        [XmlElement("last_updated")]
        public string last_updated { get; set; }

        [XmlElement("supplier")]
        public string supplier { get; set; }
    }
}
