using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double result = Math.Sqrt(Double.Parse(args[0]));

            using FileStream fs = File.OpenWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wynik.txt"));
            byte[] bytes = Encoding.UTF8.GetBytes(result.ToString());
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}
