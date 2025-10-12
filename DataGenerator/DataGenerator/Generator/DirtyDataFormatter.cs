using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator
{
    public static class DirtyDataFormatter
    {
        private static Random random = new Random();

        public static string[] DataFormats = new string[]
        {
            "dd/MM/yyyy HH:mm",           // 01/10/2025 10:30
            "yyyy-MM-ddTHH:mm:ssZ",       // 2025-10-02T14:45:00Z
            "dd-MM-yyyy HH:mm:ss",        // 03-10-2025 09:15:00
            "yyyy/MM/dd HH:mm",           // 2025/10/04 16:20
            "dd/MM/yyyy"                  // 05/10/2025
        };

        public static string FormatDirtyPrice(decimal price)
        {
            var format = random.Next(0, 5);
            return format switch
            {
                0 => $"{price}€",
                1 => $"EUR {price}",           // EUR 1199.99
                2 => $"{price}",               // 1.499,99
                3 => $"{price} EUR",           // 449.99 EUR
                4 => $"€{price}",              // €129,99
                _ => price.ToString()
            };
        }

        public static string AddSpaces(string text)
        { 
            var op = random.Next(0, 4);
            return op switch
            {
                0 => $"  {text}",                // Espaços no início
                1 => $"{text}  ",                // Espaços no fim
                2 => $"  {text}  ",              // Espaços em ambos
                _ => text
            };
        }

        public static string QuantityFormater(int quantity) 
        {
            return random.Next(0, 4) switch
            {
                0 => quantity.ToString(),
                1 => $"{quantity} un",
                2 => $"{quantity} un.",
                _ => quantity == 1 ? "1 unidade" : $"{quantity} unidades"
            };
        }

        public static string SKUFormater(string sku)
        {
            return random.Next(0, 5) switch
            {
                0 => AddSpaces(sku),
                1 => sku.Replace("-", "--"),
                2 => sku.Replace("-", "/"),
                3 => sku.Replace("-", "_"),
                _ => sku.Replace("-", " ")
            };
        }
        public static string IDFormater(string id, string OGSeparator = "-")
        {
            return random.Next(0, 4) switch
            {
                0 => AddSpaces(id),
                1 => id.Replace(OGSeparator, ""),
                2 => id.Replace(OGSeparator, $" {OGSeparator}"),
                _ => id
            };
        }
    }
}
