using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator
{
    /// <summary>
    /// Classe estática responsável por gerar dados "sujos" (dirty data) com formatos inconsistentes.
    /// Simula problemas comuns em dados reais como espaços extras, formatos variados e separadores diferentes.
    /// Útil para testar pipelines de limpeza de dados e processos ETL.
    /// </summary>
    public static class DirtyDataFormatter
    {
        // Gerador de números aleatórios compartilhado por todos os métodos
        // Declarado como static para garantir que seja único por execução
        private static Random random = new Random();

        /// <summary>
        /// Array com diferentes formatos de data/hora para simular inconsistência de dados.
        /// Cada formato representa um padrão diferente que pode aparecer em dados reais.
        /// </summary>
        public static string[] DataFormats = new string[]
        {
            "dd/MM/yyyy HH:mm",           // Formato português: 01/10/2025 10:30
            "yyyy-MM-ddTHH:mm:ssZ",       // Formato ISO 8601: 2025-10-02T14:45:00Z
            "dd-MM-yyyy HH:mm:ss",        // Formato com hífens: 03-10-2025 09:15:00
            "yyyy/MM/dd HH:mm",           // Formato japonês/chinês: 2025/10/04 16:20
            "dd/MM/yyyy"                  // Formato simplificado: 05/10/2025
        };

        /// <summary>
        /// Formata um preço decimal em diferentes formatos "sujos" para simular inconsistência.
        /// Retorna o preço com diferentes símbolos de moeda, posições e abreviações.
        /// </summary>
        /// <param name="price">Valor decimal do preço a ser formatado</param>
        /// <returns>String com o preço formatado de forma inconsistente</returns>
        public static string FormatDirtyPrice(decimal price)
        {
            // Gera um número aleatório entre 0 e 4 para escolher o formato
            var format = random.Next(0, 5);

            // Usa pattern matching (switch expression) para retornar diferentes formatos
            return format switch
            {
                0 => $"{price}€",              // Formato: 1199.99€
                1 => $"EUR {price}",           // Formato: EUR 1199.99
                2 => $"{price}",               // Formato: 1199.99 (sem moeda)
                3 => $"{price} EUR",           // Formato: 449.99 EUR
                4 => $"€{price}",              // Formato: €129.99
                _ => price.ToString()          // Fallback: apenas o número
            };
        }

        /// <summary>
        /// Adiciona espaços em branco aleatórios antes, depois ou em ambos os lados do texto.
        /// Simula problemas comuns de limpeza de dados como whitespace indesejado.
        /// </summary>
        /// <param name="text">Texto a ser modificado</param>
        /// <returns>Texto com espaços extras adicionados aleatoriamente</returns>
        public static string AddSpaces(string text)
        {
            // Gera um número aleatório entre 0 e 3 para decidir onde adicionar espaços
            var op = random.Next(0, 4);

            return op switch
            {
                0 => $"  {text}",              // Adiciona 2 espaços no início
                1 => $"{text}  ",              // Adiciona 2 espaços no fim
                2 => $"  {text}  ",            // Adiciona 2 espaços em ambos os lados
                _ => text                      // Retorna o texto sem modificação
            };
        }

        /// <summary>
        /// Formata quantidade como número inteiro ou com sufixos de unidade variados.
        /// Cria inconsistências típicas de entrada manual de dados.
        /// </summary>
        /// <param name="quantity">Quantidade numérica a ser formatada</param>
        /// <returns>String com a quantidade em formato inconsistente</returns>
        public static string QuantityFormater(int quantity)
        {
            return random.Next(0, 4) switch
            {
                0 => quantity.ToString(),                                        // Formato: "5"
                1 => $"{quantity} un",                                          // Formato: "5 un"
                2 => $"{quantity} un.",                                         // Formato: "5 un."
                _ => quantity == 1 ? "1 unidade" : $"{quantity} unidades"     // Formato: "1 unidade" ou "5 unidades"
            };
        }

        /// <summary>
        /// Formata códigos SKU (Stock Keeping Unit) com diferentes separadores.
        /// Simula variações em códigos de produto vindos de diferentes sistemas.
        /// </summary>
        /// <param name="sku">Código SKU original (ex: "PROD-123-XYZ")</param>
        /// <returns>SKU formatado com separadores inconsistentes ou espaços extras</returns>
        public static string SKUFormater(string sku)
        {
            return random.Next(0, 5) switch
            {
                0 => AddSpaces(sku),                // Adiciona espaços: "  PROD-123-XYZ"
                1 => sku.Replace("-", "--"),        // Duplica hífens: "PROD--123--XYZ"
                2 => sku.Replace("-", "/"),         // Substitui por barra: "PROD/123/XYZ"
                3 => sku.Replace("-", "_"),         // Substitui por underscore: "PROD_123_XYZ"
                _ => sku.Replace("-", " ")          // Substitui por espaço: "PROD 123 XYZ"
            };
        }

        /// <summary>
        /// Formata IDs genéricos com diferentes separadores ou espaços.
        /// Útil para simular IDs de vendas, armazéns, clientes, etc. com formatos inconsistentes.
        /// </summary>
        /// <param name="id">ID original a ser formatado (ex: "SALE-2025-001")</param>
        /// <param name="OGSeparator">Separador original usado no ID (padrão: "-")</param>
        /// <returns>ID formatado de forma inconsistente</returns>
        public static string IDFormater(string id, string OGSeparator = "-")
        {
            return random.Next(0, 4) switch
            {
                0 => AddSpaces(id),                                      // Adiciona espaços: "  SALE-2025-001"
                1 => id.Replace(OGSeparator, ""),                       // Remove separador: "SALE2025001"
                2 => id.Replace(OGSeparator, $" {OGSeparator}"),       // Adiciona espaço antes: "SALE -2025 -001"
                _ => id                                                  // Retorna sem alteração
            };
        }
    }
}
