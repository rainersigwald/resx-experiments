using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResXResourceReaderPlayground
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Debugger.Launch();
            Console.WriteLine("yo");

            var resources = new ResXResourceReader("test.resx").GetEnumerator();

            while (resources.MoveNext())
            {
                Console.WriteLine($"{resources.Key}: type={resources.Value.GetType()}, value=\"{resources.Value.ToString()}\"");
            }
        }
    }
}
