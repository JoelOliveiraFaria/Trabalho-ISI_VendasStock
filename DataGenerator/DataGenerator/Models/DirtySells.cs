using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataGenerator.Models
{
    [XmlType("sale")]
    public class DirtySells
    {
        [XmlElement("sale_id")]
        public string sale_id {  get; set; }

        [XmlElement("sale_date")]
        public string sale_date { get; set; }

        [XmlElement("customer_id")]
        public string customer_id { get; set; }

        [XmlElement("customer_email")]
        public string customer_email { get; set; }

        [XmlElement("sku")]
        public string sku { get; set; }

        [XmlElement("product_name")]
        public string product_name { get; set; }

        [XmlElement("quantity")]
        public string quantity { get; set; }

        [XmlElement("unit_price")]
        public string unit_price { get; set; }

        [XmlElement("total_amount")]
        public string total_amount { get; set; }

        [XmlElement("status")]
        public string status { get; set; }
    }
}
