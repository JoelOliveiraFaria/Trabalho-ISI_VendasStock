using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;  // Biblioteca Faker para geração de dados fictícios
using DataGenerator.Models;

namespace DataGenerator.Generator
{
    /// <summary>
    /// Classe responsável por gerar dados de inventário de stock com qualidade "suja" (dirty).
    /// Simula registos de armazém com formatos inconsistentes típicos de sistemas de gestão
    /// de inventário desintegrados ou com entrada manual de dados.
    /// </summary>
    public class StockGenerator
    {
        // Gerador de números aleatórios para decisões de formatação
        private static Random random = new Random();

        /// <summary>
        /// Catálogo de produtos disponíveis em stock.
        /// Cada produto contém: Nome, SKU (código único) e Fornecedor.
        /// Representa o inventário típico de uma loja de tecnologia.
        /// </summary>
        private static readonly (string Name, string SKU, string Supplier)[] Products = new[]
        {
            ("ASUS VivoBook 15 Laptop", "ERP-LAP-001-2024", "ASUS Inc"),
            ("Samsung Galaxy S24 Ultra", "ERP-PHONE-002-2024", "Samsung Electronics"),
            ("Apple iPad Pro 12.9 512GB", "ERP-TAB-003-2024", "Apple Inc."),
            ("LG UltraWide 34\" Monitor", "ERP-MON-004-2024", "LG Electronics"),
            ("Logitech MX Keys", "ERP-KEY-005-2024", "Logitech"),
            ("Logitech MX Master 3S", "ERP-MOU-006-2024", "Logitech International")
        };

        /// <summary>
        /// Array de localizações físicas dentro dos armazéns.
        /// Inclui versões com capitalização inconsistente (minúsculas e maiúsculas)
        /// para simular dados introduzidos manualmente por diferentes operadores.
        /// </summary>
        private static readonly string[] Locations = new[]
        {
            "Warehouse A - Section 1",      // Capitalização correta
            "WAREHOUSE A - SECTION 1",      // Tudo em maiúsculas
            "Warehouse B - Section 2",      // Capitalização correta
            "WAREHOUSE B - SECTION 2",      // Tudo em maiúsculas
        };

        /// <summary>
        /// Método principal que gera uma lista de registos de stock com dados "sujos".
        /// Cria dados fictícios de inventário com inconsistências típicas de sistemas
        /// de gestão de armazém, incluindo formatos variados de IDs, quantidades e datas.
        /// </summary>
        /// <param name="quantity">Número de registos de stock a serem gerados</param>
        /// <returns>Lista de objetos DirtyStock com dados inconsistentes</returns>
        public static List<DirtyStock> Generate(int quantity)
        {
            // Configura o Faker com locale português de Portugal
            var stockFaker = new Faker<DirtyStock>("pt_PT")

                // Gera ID de armazém no formato WH-XX (Warehouse-número) com formatação inconsistente
                .RuleFor(s => s.warehouse_id, f =>
                {
                    // Gera ID aleatório entre WH-01 e WH-05
                    // :D2 formata o número com 2 dígitos (padding de zero)
                    var id = $"WH-{f.Random.Int(1, 5):D2}";

                    // Aplica 3 variações aleatórias de formatação
                    return random.Next(0, 3) switch
                    {
                        0 => id,                        // Original: WH-01
                        1 => id.Replace("-", "_"),      // Com underscore: WH_01
                        _ => DirtyDataFormatter.AddSpaces(id)  // Com espaços extras: "  WH-01  "
                    };
                })

                // Seleciona um produto aleatório e formata o SKU de forma inconsistente
                .RuleFor(s => s.sku, f =>
                {
                    // Escolhe um produto aleatório do catálogo
                    var product = f.PickRandom(Products);
                    // Aplica formatação "suja" ao SKU (altera separadores)
                    return DirtyDataFormatter.SKUFormater(product.SKU);
                })

                // Seleciona um produto aleatório e adiciona espaços extras ao nome
                .RuleFor(s => s.product_name, f =>
                {
                    // Escolhe um produto aleatório do catálogo
                    var produto = f.PickRandom(Products);
                    // Adiciona espaços no início, fim ou ambos
                    return DirtyDataFormatter.AddSpaces(produto.Name);
                })

                // Gera quantidade disponível entre 0 e 500 com formatos textuais variados
                .RuleFor(s => s.quantity_available, f =>
                {
                    // Gera quantidade aleatória (0 simula stock esgotado)
                    var qty = f.Random.Int(0, 500);

                    // Aplica 3 formatos diferentes de representação de quantidade
                    return random.Next(0, 3) switch
                    {
                        0 => qty.ToString(),     // Formato: "350"
                        1 => $"{qty} units",     // Formato: "350 units"
                        _ => $"{qty}pcs"         // Formato: "350pcs" (sem espaço)
                    };
                })

                // Gera nível mínimo de stock entre 10 e 50 unidades (sempre como string numérica)
                .RuleFor(s => s.minimum_level, f => f.Random.Int(10, 50).ToString())

                // Escolhe aleatoriamente uma localização da lista (com capitalização inconsistente)
                .RuleFor(s => s.location, f => f.PickRandom(Locations))

                // Gera data de última atualização entre 01/09/2025 e 12/10/2025 com formatos variados
                .RuleFor(s => s.last_updated, f =>
                {
                    // Gera uma data aleatória dentro do intervalo especificado
                    var data = f.Date.Between(DateTime.Parse("2025-09-01"), DateTime.Parse("2025-10-12"));
                    // Escolhe aleatoriamente um dos formatos de data disponíveis
                    // e formata a data nesse padrão
                    return data.ToString(f.PickRandom(DirtyDataFormatter.DataFormats));
                })

                // Seleciona o fornecedor correspondente ao produto e adiciona espaços extras
                .RuleFor(s => s.supplier, f =>
                {
                    // Escolhe um produto aleatório do catálogo
                    var product = f.PickRandom(Products);
                    // Adiciona espaços extras ao nome do fornecedor
                    return DirtyDataFormatter.AddSpaces(product.Supplier);
                });

            // Gera a quantidade especificada de registos de stock e retorna como lista
            return stockFaker.Generate(quantity);
        }
    }
}
