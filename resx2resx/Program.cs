using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace resx2resx
{
    class Program
    {
        static void Main(string[] args)
        {
            var resources = new Dictionary<string, object>();

            // read resx

            // TODO: support custom types by creating a resolver
            ITypeResolutionService typeResolutionService = null;

            using (ResXResourceReader reader = new ResXResourceReader(args[0]) { UseResXDataNodes = true })
            {
                var resEnum = reader.GetEnumerator();

                while (resEnum.MoveNext())
                {
                    string name = (string)resEnum.Key;
                    var node = (ResXDataNode)resEnum.Value;

                    var value = node.GetValue(typeResolutionService);
                    resources.Add(name, value);
                }
            }

            // write updated resx

            var doc = new XmlDocument() { PreserveWhitespace = true };
            doc.Load(args[0]);

            foreach (var typeConverterSerializedDataNode in doc.SelectNodes($"//data[@mimetype=\"{ResXResourceWriter.ByteArraySerializedObjectMimeType}\"]"))
            {
                var nodeToUpdate = (XmlNode)typeConverterSerializedDataNode;

                var resourceName = nodeToUpdate.Attributes["name"].Value;

                nodeToUpdate.Attributes["mimetype"].Value = ResXResourceWriter.BinSerializedObjectMimeType;

                nodeToUpdate["value"].InnerText = ArmoredBinaryFormattedValue(resources[resourceName]);
            }

            doc.Save("output.resx");
        }

        private static string ArmoredBinaryFormattedValue(object value)
        {
            // serialize object "like resourcewriter"
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, value);

            // and return it in an armored array "like resxwriter"
            return ToBase64WrappedString(ms.ToArray());
        }

        private static string ToBase64WrappedString(byte[] data)
        {
            const int lineWrap = 80;
            const string crlf = "\r\n";
            const string prefix = "        ";
            string raw = Convert.ToBase64String(data);
            if (raw.Length > lineWrap)
            {
                StringBuilder output = new StringBuilder(raw.Length + (raw.Length / lineWrap) * 3); // word wrap on lineWrap chars, \r\n
                int current = 0;
                for (; current < raw.Length - lineWrap; current += lineWrap)
                {
                    output.Append(crlf);
                    output.Append(prefix);
                    output.Append(raw, current, lineWrap);
                }
                output.Append(crlf);
                output.Append(prefix);
                output.Append(raw, current, raw.Length - current);
                output.Append(crlf);
                return output.ToString();
            }

            return raw;
        }

    }
}
