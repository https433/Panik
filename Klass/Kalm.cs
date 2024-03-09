using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panik.Klass
{
    public class Kalm
    {
        public static void Success(StreamWriter writer, Process process)
        {
            process.Kill();
            writer.WriteLine($"Terminated process: {process.ProcessName} (PID: {process.Id})");
            Console.WriteLine($"Terminated process: {process.ProcessName} (PID: {process.Id})");
        }
    }
}
