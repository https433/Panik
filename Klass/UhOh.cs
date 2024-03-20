using System.Diagnostics;

namespace Panik.Klass
{
    public class UhOh
    {
        public static void Fail(StreamWriter writer, Process process, Exception ex)
        {
            writer.WriteLine($"Failed to terminate process {process.ProcessName}: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"Failed to terminate process {process.ProcessName}: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
