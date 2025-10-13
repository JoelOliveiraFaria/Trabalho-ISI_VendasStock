using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using DataGenerator.Models;

namespace DataGenerator.Generator
{
    public class StockGenerator
    {
        private static Random random = new Random();

        private static readonly (string Name, string SKU, string Supplier)[] Products = new[]
        {
            ("ASUS VivoBook 15 Laptop", "ERP-LAP-001-2024", "ASUS Inc"),
            ("Samsung Galaxy S24 Ultra", "ERP-PHONE-002-2024", "Samsung Electronics"),
            ("Apple iPad Pro 12.9 512GB", "ERP-TAB-003-2024", "Apple Inc."),
            ("LG UltraWide 34\" Monitor", "ERP-MON-004-2024", "LG Electronics"),
            ("Logitech MX Keys", "ERP-KEY-005-2024", "Logitech"),
            ("Logitech MX Master 3S", "ERP-MOU-006-2024", "Logitech International")
        };

        private static readonly string[] Locations = new[]
        {
            "Warehouse A - Section 1", "WAREHOUSE A - SECTION 1",
            "Warehouse B - Section 2", "WAREHOUSE B - SECTION 2",
        };

        public static List<DirtyStock> Generate(int quantity)
        {
            var stockFaker = new Faker<DirtyStock>("pt_PT")
                .RuleFor(s => s.warehouse_id, f =>
                {
                    var id = $"WH-{f.Random.Int(1, 5):D2}";
                    return random.Next(0, 3) switch
                    {
                        0 => id,
                        1 => id.Replace("-", "_"),
                        _ => DirtyDataFormatter.AddSpaces(id)
                    };
                })
                .RuleFor(s => s.sku, f =>
                {
                    var product = f.PickRandom(Products);
                    return DirtyDataFormatter.SKUFormater(product.SKU);
                })
                .RuleFor(s => s.product_name, f =>
                {
                    var produto = f.PickRandom(Products);
                    return DirtyDataFormatter.AddSpaces(produto.Name);
                })
                .RuleFor(s => s.quantity_available, f =>
                {
                    var qty = f.Random.Int(0, 500);
                    return random.Next(0, 3) switch
                    {
                        0 => qty.ToString(),
                        1 => $"{qty} units",
                        _ => $"{qty}pcs"
                    };
                })
                .RuleFor(s => s.minimum_level, f => f.Random.Int(10, 50).ToString())
                .RuleFor(s => s.location, f => f.PickRandom(Locations))
                .RuleFor(s => s.last_updated, f =>
                {
                    var data = f.Date.Between(DateTime.Parse("2025-09-01"), DateTime.Parse("2025-10-12"));
                    return data.ToString(f.PickRandom(DirtyDataFormatter.DataFormats));
                })
                .RuleFor(s => s.supplier, f =>
                {
                    var product = f.PickRandom(Products);
                    return DirtyDataFormatter.AddSpaces(product.Supplier);
                });

            return stockFaker.Generate(quantity);
        }
    }
}
