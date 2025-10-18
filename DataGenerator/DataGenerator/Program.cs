using DataGenerator.Generator;
using DataGenerator.Exporter;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cabeçalho da aplicação
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  Gerador de Dados Sujos - Vendas e Stock");
            Console.WriteLine("  Trabalho Prático ISI 2025/26");
            Console.WriteLine("═══════════════════════════════════════════════════════════\n");

            // Diretório de saída para os ficheiros gerados
            string DirectoryOutput = @"C:\Users\joelo_k10i33y\Documents\GitHub\Trabalho-ISI_VendasStock\Knime\data\input";

            try
            {
                // Caminhos completos para os ficheiros de vendas
                string JsonSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.json");
                string CsvSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.csv");
                string XMLSalles = Path.Combine(DirectoryOutput, "ecommerce_sales_dirty.xml");

                // Caminhos completos para os ficheiros de stock
                string stockJson = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.json");
                string stockCsv = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.csv");
                string stockXml = Path.Combine(DirectoryOutput, "warehouse_stock_dirty.xml");

                // Geração de dados
                Console.WriteLine("A gerar dados");
                var sells = Generator.SellsGenerator.Generate(70); // 70 vendas
                Console.WriteLine($"{sells.Count} vendas geradas (com inconsistências)");

                var stock = StockGenerator.Generate(70); // 70 registos de stock
                Console.WriteLine($"✓ {stock.Count} registros de stock gerados (com inconsistências)\n");

                // Exportação para JSON
                Console.WriteLine("A exportar para JSON...");
                Exporter.DataExporter.ExportJSON(sells, JsonSalles, "ecommerce_sales");
                Exporter.DataExporter.ExportJSON(stock, stockJson, "warehouse_stock");
                Console.WriteLine("✓ Arquivos JSON criados");

                // Exportação para CSV
                Console.WriteLine("A exportar para CSV...");
                Exporter.DataExporter.ExportCSV(sells, CsvSalles);
                Exporter.DataExporter.ExportCSV(stock, stockCsv);
                Console.WriteLine("✓ Arquivos CSV criados");

                // Exportação para XML
                Console.WriteLine("Exportando para XML...");
                Exporter.DataExporter.ExportXML(sells, XMLSalles, "ecommerce_sales");
                Exporter.DataExporter.ExportXML(stock, stockXml, "warehouse_stock");
                Console.WriteLine("✓ Arquivos XML criados\n");

                // Mensagem de sucesso
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine("  Arquivos gerados com sucesso!");
                Console.WriteLine("═══════════════════════════════════════════════════════════\n");
            }
            catch (Exception ex)
            {
                // Exibe erro completo se algo falhar
                Console.WriteLine(ex.ToString());
            }

            // Aguarda tecla antes de fechar
            Console.ReadKey();
        }
    }
}
