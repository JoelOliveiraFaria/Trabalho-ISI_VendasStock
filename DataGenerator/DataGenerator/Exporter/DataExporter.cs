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
    public static class DataExporter
    {
        public static void ExportJSON<T>(List<T> data, string filename, string rootName)
        {
            var wrapper = new Dictionary<string, object>
            {
                { rootName, data }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonString = JsonSerializer.Serialize(wrapper, options);
            File.WriteAllText(filename, jsonString);
        }

        public static void ExportCSV<T>(List<T> data, string filename)
        {
            var properties = typeof(T).GetProperties();

            using (var writer = new StreamWriter(filename))
            {
                // Cabeçalho
                writer.WriteLine(string.Join(";", System.Linq.Enumerable.Select(properties, p => p.Name)));

                // Dados
                foreach (var item in data)
                {
                    var valores = System.Linq.Enumerable.Select(properties, p => p.GetValue(item)?.ToString() ?? "");
                    writer.WriteLine(string.Join(";", valores));
                }
            }
        }

        public static void ExportXML<T>(List<T> data, string filename, string rootElementName)
        {
            // Criar um wrapper para definir o nome do elemento raiz
            var xmlRoot = new XmlRootAttribute(rootElementName);
            var serializer = new XmlSerializer(typeof(List<T>), xmlRoot);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (var writer = XmlWriter.Create(filename, settings))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
