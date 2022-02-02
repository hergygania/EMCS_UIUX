using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("====== RUNNING PROCESS ======");
            System.Console.WriteLine("====== Read Email Exchange======");

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Library.Library.ReadEmailExchange();            
            sw.Stop();

            System.Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            System.Console.WriteLine("====== PROCESS DONE ======");
            System.Console.ReadKey();
        }
    }
}
