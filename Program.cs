using Panik.Klass;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Panik
{
    public record App(
        [property: JsonPropertyName("_key")] string _key,
        [property: JsonPropertyName("_name")] string _name,
        [property: JsonPropertyName("_isforced")] bool _isforced
    );

    public record Program(
        [property: JsonPropertyName("app")] IReadOnlyList<App> app
    );

    public record Root(
        [property: JsonPropertyName("v")] int v,
        [property: JsonPropertyName("logdir")] string logdir,
        [property: JsonPropertyName("program")] Program program
    );

    public class Klean
    {
        [RequiresDynamicCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(String, JsonSerializerOptions)")]
        [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(String, JsonSerializerOptions)")]
        static void Main()
        {
            string AppSetings = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (!File.Exists(AppSetings))
                ThrwCslErr($"{AppSetings} doesnt exist!");

            Json(out string json);
            Root ProgramSettings = JsonSerializer.Deserialize<Root>(json);

            if (ProgramSettings?.v == null)
                ThrwCslErr($"Error In Settings.json {ProgramSettings?.v} is null!");
            if (ProgramSettings?.v is > 1 or 0)
                ThrwCslErr("Invalid Settings Version");



            string LogFil = "ProcessTerminationLog.txt";
            string[] NoKil = {
                "cmd",
                "explorer",
                "taskmgr",
                "panik",
                "proton",
                "proton vpn",
                "NLClientApp",
                "FL64",
                "xampp",
                "ProcessLasso",
                "vpn",
                "tailscale",
                "chrome",
                "firefox",
                "5795795798567957",
                "fences",
                "dwm",
                "discord",
                "devenv",
                "svchost",
                "brave",
                "conhost",
                "ctfmon",
                "sihost",
                "nvcontainer",
                "Microsoft.ServiceHub.Controller",
                "dllhost",
                "StartMenuExperienceHost",
                "ServiceHub.IdentityHost",
                "ServiceHub.VSDetouredHost",
                "SearchHost",
                "RuntimeBroker",
                "Widgets",
                "dllhost",
                "WmiPrvSE",
                "NvOAWrapperCache",
                "ServiceHub.Host.dotnet.x64",
                "TextInputHost",
                "MSBuild",
                "VBCSCompiler",
                "VsDebugConsole"
            };

            Process[] Proces = Process.GetProcesses();
            using (StreamWriter writer = new StreamWriter(LogFil, append: false))
            {
                foreach (Process process in Proces)
                {
                    string PanikKILLNam = process.ProcessName.ToLower();
                    if (!NoKil.Any(excluded => PanikKILLNam.Equals(excluded.ToLower())))
                    {
                        try
                        {
                            Kalm.Success(writer, process);
                        }
                        catch (Exception ex)
                        {
                            UhOh.Fail(writer, process, ex);
                        }
                    }
                }
            }
            Console.WriteLine($"Process termination complete. Log written to: {LogFil}");
            Console.ReadKey();

            static void Json(out string json)
            {
                StreamReader r = new StreamReader("settings.json");
                json = r.ReadToEnd();
            }
        }

        private static void ThrwCslErr(in string e)
        {
            Console.WriteLine(e);
            Environment.Exit(0);
        }
        //panik
    }
}

