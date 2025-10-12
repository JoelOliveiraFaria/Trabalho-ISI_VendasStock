using DataGenerator.Generator;
using DataGenerator.Exporter;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  Gerador de Dados Sujos - Vendas e Stock");
            Console.WriteLine("  Trabalho Prático ISI 2025/26");
            Console.WriteLine("═══════════════════════════════════════════════════════════\n");

            string DirectoryOutput = @"C:\Users\joelo_k10i33y\Documents\GitHub\Trabalho-ISI_VendasStock\data\input";

            try
            {
                string JsonSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.json");
                string CsvSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.csv");
                string XMLSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.xml");
                string stockJson = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.json");
                string stockCsv = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.csv");
                string stockXml = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.xml");

                Console.WriteLine("A gerar dados");
                var sells = Generator.SellsGenerator.Generate(200);
                Console.WriteLine($"{sells.Count} vendas geradas (com inconsistências)");

                var stock = StockGenerator.Generate(100);
                Console.WriteLine($"✓ {stock.Count} registros de stock gerados (com inconsistências)\n");

                Console.WriteLine("A exportar para JSON...");
                Exporter.DataExporter.ExportJSON(sells, JsonSalles, "ecommerce_sales");
                Exporter.DataExporter.ExportJSON(stock, stockJson, "warehouse_stock");
                Console.WriteLine("✓ Arquivos JSON criados");

                Console.WriteLine("A exportar para CSV...");
                Exporter.DataExporter.ExportCSV(sells, CsvSalles);
                Exporter.DataExporter.ExportCSV(stock, stockCsv);
                Console.WriteLine("✓ Arquivos CSV criados");

                // Exportar para XML
                Console.WriteLine("Exportando para XML...");
                Exporter.DataExporter.ExportXML(sells, XMLSalles, "ecommerce_sales");
                Exporter.DataExporter.ExportXML(stock, stockXml, "warehouse_stock");
                Console.WriteLine("✓ Arquivos XML criados\n");

                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine("  Arquivos gerados com sucesso!");
                Console.WriteLine("═══════════════════════════════════════════════════════════\n");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}