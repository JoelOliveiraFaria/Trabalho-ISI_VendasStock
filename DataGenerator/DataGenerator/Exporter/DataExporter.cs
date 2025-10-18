using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DataGenerator.Exporter
{
    /// <summary>
    /// Classe estática responsável por exportar dados genéricos para múltiplos formatos de ficheiro.
    /// Suporta exportação para JSON, CSV e XML.
    /// </summary>
    public static class DataExporter
    {
        /// <summary>
        /// Exporta uma lista de objetos genéricos para um ficheiro JSON.
        /// Cria um wrapper com o nome raiz especificado e formata o JSON com indentação.
        /// </summary>
        /// <typeparam name="T">Tipo genérico dos dados a serem exportados</typeparam>
        /// <param name="data">Lista de objetos a serem exportados</param>
        /// <param name="filename">Caminho completo do ficheiro de destino (ex: "data.json")</param>
        /// <param name="rootName">Nome do elemento raiz no JSON (ex: "sales", "products")</param>
        public static void ExportJSON<T>(List<T> data, string filename, string rootName)
        {
            // Cria um wrapper (embrulho) para os dados com o nome raiz especificado
            // Isto permite ter uma estrutura JSON como: { "sales": [ {...}, {...} ] }
            var wrapper = new Dictionary<string, object>
            {
                { rootName, data }
            };

            // Configura as opções de serialização JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,  // Formata o JSON com indentação para melhor legibilidade
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping  // Permite caracteres especiais sem escape
            };

            // Converte o wrapper para string JSON usando as opções configuradas
            string jsonString = JsonSerializer.Serialize(wrapper, options);

            // Escreve o conteúdo JSON no ficheiro especificado
            File.WriteAllText(filename, jsonString);
        }

        /// <summary>
        /// Exporta uma lista de objetos genéricos para um ficheiro CSV.
        /// Utiliza ponto e vírgula (;) como delimitador de campos.
        /// A primeira linha contém os nomes das propriedades como cabeçalho.
        /// </summary>
        /// <typeparam name="T">Tipo genérico dos dados a serem exportados</typeparam>
        /// <param name="data">Lista de objetos a serem exportados</param>
        /// <param name="filename">Caminho completo do ficheiro de destino (ex: "data.csv")</param>
        public static void ExportCSV<T>(List<T> data, string filename)
        {
            // Obtém todas as propriedades públicas do tipo T usando Reflection
            var properties = typeof(T).GetProperties();

            // Usa StreamWriter para escrever no ficheiro linha a linha
            using (var writer = new StreamWriter(filename))
            {
                // Escreve o cabeçalho CSV com os nomes das propriedades
                // Exemplo: "ID;Nome;Preço;Quantidade"
                writer.WriteLine(string.Join(";", System.Linq.Enumerable.Select(properties, p => p.Name)));

                // Itera sobre cada objeto na lista de dados
                foreach (var item in data)
                {
                    // Obtém os valores de todas as propriedades do objeto atual
                    // Se o valor for null, substitui por string vazia
                    var valores = System.Linq.Enumerable.Select(properties, p => p.GetValue(item)?.ToString() ?? "");

                    // Escreve uma linha CSV com os valores separados por ponto e vírgula
                    writer.WriteLine(string.Join(";", valores));
                }
            }
        }

        /// <summary>
        /// Exporta uma lista de objetos genéricos para um ficheiro XML.
        /// Cria um documento XML formatado com indentação e um elemento raiz personalizado.
        /// </summary>
        /// <typeparam name="T">Tipo genérico dos dados a serem exportados</typeparam>
        /// <param name="data">Lista de objetos a serem exportados</param>
        /// <param name="filename">Caminho completo do ficheiro de destino (ex: "data.xml")</param>
        /// <param name="rootElementName">Nome do elemento raiz no XML (ex: "Sales", "Products")</param>
        public static void ExportXML<T>(List<T> data, string filename, string rootElementName)
        {
            // Cria um atributo XmlRoot para definir o nome do elemento raiz do XML
            // Isto permite ter uma estrutura XML como: <Sales>...</Sales>
            var xmlRoot = new XmlRootAttribute(rootElementName);

            // Cria um serializador XML para o tipo List<T> com o elemento raiz customizado
            var serializer = new XmlSerializer(typeof(List<T>), xmlRoot);

            // Configura as definições de formatação do XML
            var settings = new XmlWriterSettings
            {
                Indent = true,              // Ativa a indentação para melhor legibilidade
                IndentChars = "  ",         // Define dois espaços como indentação
                NewLineChars = "\n",        // Define quebra de linha Unix-style
                NewLineHandling = NewLineHandling.Replace  // Substitui todas as quebras de linha pelo padrão definido
            };

            // Cria um XmlWriter com as configurações definidas e serializa os dados
            using (var writer = XmlWriter.Create(filename, settings))
            {
                // Serializa a lista de dados para XML e escreve no ficheiro
                serializer.Serialize(writer, data);
            }
        }
    }
}
