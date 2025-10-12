using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace DataGenerator.Generator
{
    public static class SellsGenerator
    {
        private static Random random = new Random();

        private static readonly (string Name, string Category, string SKU, decimal Price)[] Products = new[]
        {
            ("ASUS VivoBook 15 Laptop", "laptops", "ERP-LAP-001-2024", 599.99m),
            ("Samsung Galaxy S24 Ultra", "smartphones", "ERP-PHONE-002-2024", 1199.99m),
            ("Apple iPad Pro 12.9 512GB", "tablets", "ERP-TAB-003-2024", 1499.99m),
            ("LG UltraWide 34\" Monitor", "monitors", "ERP-MON-004-2024", 449.99m),
            ("Logitech MX Keys Mechanical Keyboard", "peripherals", "ERP-KEY-005-2024", 129.99m),
            ("Logitech MX Master 3S Mouse", "peripherals", "ERP-MOU-006-2024", 99.99m)
        };

        private static readonly string[] SellsStatus = new[]
        {
            "COMPLETED", "completed", "Completed", "pending", "processing", "PENDING"
        };

        public static List<DirtySells> Generate(int quantity)
        {
            var FakerSell = new Faker<DirtySells>("pt_PT")
                .RuleFor(v => v.sale_id, f =>
                {
                    var id = $"ECO-{f.IndexFaker + 1:D3}-2025";
                    return DirtyDataFormatter.IDFormater(id);
                })
                .RuleFor(v => v.sale_date, f =>
                {
                    var date = f.Date.Between(DateTime.Parse("2025-10-01"), DateTime.Parse("2025-10-10"));
                    var format = f.PickRandom(DirtyDataFormatter.DataFormats);
                    return date.ToString(format);
                })
                .RuleFor(v => v.customer_id, f =>
                {
                    var id = $"CUST-{1000 + f.IndexFaker + 1}";
                    return random.Next(0, 3) switch
                    {
                        0 => id,
                        1 => id.Replace("-", " "),
                        _ => id.Replace("-", "_")
                    };
                })
                .RuleFor(v => v.customer_email, f =>
                {
                    var email = f.Internet.Email().ToLower();
                    return random.Next(0, 3) switch
                    {
                        0 => DirtyDataFormatter.AddSpaces(email),
                        1 => email.Replace("@", ".."),
                        _ => email
                    };
                })
                .RuleFor(v => v.sku, f =>
                {
                    var product = f.PickRandom(Products);
                    return DirtyDataFormatter.SKUFormater(product.SKU);
                })
                .RuleFor(v => v.product_name, f =>
                {
                    var product = f.PickRandom(Products);
                    return DirtyDataFormatter.AddSpaces(product.Name);
                })
                .RuleFor(v => v.category, f =>
                {
                    var categories = new[]
                    {
                        "laptops", "LAPTOPS", "Laptops",
                        "smartphones", "SMARTPHONES",
                        "tablets", "TABLETS", "Tablets",
                        "monitors", "MONITORS",
                        "peripherals", "PERIPHERALS", "Peripherals"
                    };
                    return DirtyDataFormatter.AddSpaces(f.PickRandom(categories));
                })
                .RuleFor(v => v.quantity, f =>
                {
                    var qty = f.Random.Int(1, 3);
                    return DirtyDataFormatter.QuantityFormater(qty);
                })
                .RuleFor(v => v.unit_price, f =>
                {
                    var preco = f.Finance.Amount(99, 1499);
                    return DirtyDataFormatter.FormatDirtyPrice(preco);
                })
                .RuleFor(v => v.total_amount, f =>
                {
                    var total = f.Finance.Amount(99, 2999);
                    return DirtyDataFormatter.FormatDirtyPrice(total);
                })
                .RuleFor(v => v.status, f => f.PickRandom(SellsStatus));

            return FakerSell.Generate(quantity);
        }
    }
}
