using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowService.Library;

namespace WindowsServicePOST
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("====== RUNNING PROCESS ======");
            System.Console.WriteLine("====== Read Email Exchange ======");

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Library.ReadEmailPOST();
            sw.Stop();

            System.Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            System.Console.WriteLine("====== PROCESS DONE ======");
        }
    }
}
