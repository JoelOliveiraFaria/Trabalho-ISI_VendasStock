using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;  // Biblioteca Faker para geração de dados fictícios realistas

namespace DataGenerator.Generator
{
    /// <summary>
    /// Classe estática responsável por gerar dados de vendas (sells) com qualidade "suja" (dirty).
    /// Utiliza a biblioteca Bogus para criar dados fictícios e aplica formatações inconsistentes
    /// através da classe DirtyDataFormatter para simular dados reais de sistemas desintegrados.
    /// </summary>
    public static class SellsGenerator
    {
        // Gerador de números aleatórios para decisões de formatação
        private static Random random = new Random();

        /// <summary>
        /// Catálogo de produtos disponíveis para venda.
        /// Cada produto contém: Nome, Categoria, SKU (código único) e Preço base.
        /// Esta estrutura simula um catálogo de e-commerce de tecnologia.
        /// </summary>
        private static readonly (string Name, string Category, string SKU, decimal Price)[] Products = new[]
        {
            ("ASUS VivoBook 15 Laptop", "laptops", "ERP-LAP-001-2024", 599.99m),
            ("Samsung Galaxy S24 Ultra", "smartphones", "ERP-PHONE-002-2024", 1199.99m),
            ("Apple iPad Pro 12.9 512GB", "tablets", "ERP-TAB-003-2024", 1499.99m),
            ("LG UltraWide 34\" Monitor", "monitors", "ERP-MON-004-2024", 449.99m),
            ("Logitech MX Keys Mechanical Keyboard", "peripherals", "ERP-KEY-005-2024", 129.99m),
            ("Logitech MX Master 3S Mouse", "peripherals", "ERP-MOU-006-2024", 99.99m)
        };

        /// <summary>
        /// Array de estados de venda com formatos inconsistentes.
        /// Simula dados vindos de diferentes sistemas com convenções de nomenclatura variadas
        /// (maiúsculas, minúsculas, capitalização mista).
        /// </summary>
        private static readonly string[] SellsStatus = new[]
        {
            "COMPLETED", "completed", "Completed", "pending", "processing", "PENDING"
        };

        /// <summary>
        /// Método principal que gera uma lista de vendas com dados "sujos".
        /// Utiliza Bogus Faker para criar dados realistas e aplica formatos inconsistentes
        /// intencionalmente para simular problemas comuns em dados de produção.
        /// </summary>
        /// <param name="quantity">Número de registos de venda a serem gerados</param>
        /// <returns>Lista de objetos DirtySells com dados inconsistentes</returns>
        public static List<DirtySells> Generate(int quantity)
        {
            // Configura o Faker com locale português de Portugal
            var FakerSell = new Faker<DirtySells>("pt_PT")

                // Gera ID de venda no formato ECO-XXX-2025 com formatação inconsistente
                .RuleFor(v => v.sale_id, f =>
                {
                    // Cria ID sequencial: ECO-001-2025, ECO-002-2025, etc.
                    // :D3 formata o número com 3 dígitos (padding de zeros)
                    var id = $"ECO-{f.IndexFaker + 1:D3}-2025";
                    // Aplica formatação "suja" ao ID (remove/altera separadores)
                    return DirtyDataFormatter.IDFormater(id);
                })

                // Gera data de venda entre 01/10/2025 e 10/10/2025 com formatos variados
                .RuleFor(v => v.sale_date, f =>
                {
                    // Gera uma data aleatória dentro do intervalo especificado
                    var date = f.Date.Between(DateTime.Parse("2025-10-01"), DateTime.Parse("2025-10-10"));
                    // Escolhe aleatoriamente um dos formatos de data disponíveis
                    var format = f.PickRandom(DirtyDataFormatter.DataFormats);
                    // Retorna a data formatada no formato escolhido
                    return date.ToString(format);
                })

                // Gera ID de cliente no formato CUST-1XXX com separadores variados
                .RuleFor(v => v.customer_id, f =>
                {
                    // Cria ID de cliente sequencial: CUST-1001, CUST-1002, etc.
                    var id = $"CUST-{1000 + f.IndexFaker + 1}";
                    // Aplica 3 variações aleatórias de formatação
                    return random.Next(0, 3) switch
                    {
                        0 => id,                        // Original: CUST-1001
                        1 => id.Replace("-", " "),      // Com espaço: CUST 1001
                        _ => id.Replace("-", "_")       // Com underscore: CUST_1001
                    };
                })

                // Gera email de cliente com problemas comuns (espaços, @ duplicado)
                .RuleFor(v => v.customer_email, f =>
                {
                    // Gera email fictício realista em lowercase
                    var email = f.Internet.Email().ToLower();
                    // Aplica 3 tipos de "sujidade" aleatoriamente
                    return random.Next(0, 3) switch
                    {
                        0 => DirtyDataFormatter.AddSpaces(email),  // Adiciona espaços extras
                        1 => email.Replace("@", ".."),             // Substitui @ por .. (erro de digitação)
                        _ => email                                  // Mantém email correto
                    };
                })

                // Seleciona um produto aleatório e formata o SKU de forma inconsistente
                .RuleFor(v => v.sku, f =>
                {
                    // Escolhe um produto aleatório do catálogo
                    var product = f.PickRandom(Products);
                    // Aplica formatação "suja" ao SKU do produto
                    return DirtyDataFormatter.SKUFormater(product.SKU);
                })

                // Seleciona um produto aleatório e adiciona espaços extras ao nome
                .RuleFor(v => v.product_name, f =>
                {
                    // Escolhe um produto aleatório do catálogo
                    var product = f.PickRandom(Products);
                    // Adiciona espaços no início, fim ou ambos
                    return DirtyDataFormatter.AddSpaces(product.Name);
                })

                // Gera quantidade entre 1 e 3 com formatos textuais variados
                .RuleFor(v => v.quantity, f =>
                {
                    // Gera quantidade aleatória entre 1 e 3 unidades
                    var qty = f.Random.Int(1, 3);
                    // Formata como "5", "5 un", "5 un.", "5 unidades"
                    return DirtyDataFormatter.QuantityFormater(qty);
                })

                // Gera preço unitário entre 99€ e 1499€ com formatos de moeda variados
                .RuleFor(v => v.unit_price, f =>
                {
                    // Gera um preço aleatório na faixa especificada
                    var preco = f.Finance.Amount(99, 1499);
                    // Aplica formatos inconsistentes: "599€", "EUR 599", "€599", etc.
                    return DirtyDataFormatter.FormatDirtyPrice(preco);
                })

                // Gera valor total entre 99€ e 2999€ com formatos de moeda variados
                .RuleFor(v => v.total_amount, f =>
                {
                    // Gera um valor total aleatório (normalmente seria quantity * unit_price)
                    // Nota: Aqui é gerado independentemente para adicionar mais inconsistência
                    var total = f.Finance.Amount(99, 2999);
                    // Aplica formatos inconsistentes de preço
                    return DirtyDataFormatter.FormatDirtyPrice(total);
                })

                // Escolhe aleatoriamente um estado de venda da lista de estados inconsistentes
                .RuleFor(v => v.status, f => f.PickRandom(SellsStatus));

            // Gera a quantidade especificada de registos e retorna como lista
            return FakerSell.Generate(quantity);
        }
    }
}
