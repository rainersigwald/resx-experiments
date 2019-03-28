using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace resx2resx
{
    class Program
    {
        static void Main(string[] args)
        {
            var resources = new List<(string name, object value)>();

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
                    resources.Add((name, value));

                    //// serialize object "like resourcewriter"

                    //var bf = new BinaryFormatter();
                    //using (var ms = new MemoryStream())
                    //{
                    //    bf.Serialize(ms, value);

                    //    var bytes = ms.ToArray();

                    //    resources.Add((name, bytes));
                    //}
                }
            }

            // write updated resx

            // Load as xml
            var rawDocument = XElement.Load(args[0], LoadOptions.PreserveWhitespace);

            foreach (var typeConverterSerializedDataNode in rawDocument.Descendants("data")
                .Where(d => d.Attribute("mimetype").Value == ResXResourceWriter.ByteArraySerializedObjectMimeType))
            {
                var binaryFormatterSerializedDataNode = new XElement(typeConverterSerializedDataNode);

                binaryFormatterSerializedDataNode.Attribute("mimetype").Value = ResXResourceWriter.BinSerializedObjectMimeType;

            }

            rawDocument.Save("output.txt");
        }
    }
}
