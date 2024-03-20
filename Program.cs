using Panik.Klass;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Panik
{
    public class App
    {
        [JsonPropertyName("_key")]
        public string? Key { get; set; }

        [JsonPropertyName("_name")]
        public string? Name { get; set; }

        [JsonPropertyName("_isforced")]
        public bool IsForced { get; set; }
    }

    public class Program
    {
        [JsonPropertyName("app")]
        public IReadOnlyList<App>? Apps { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("v")]
        public int Version { get; set; }

        [JsonPropertyName("logdir")]
        public string LogDir { get; set; }

        [JsonInclude]
        public Program Program { get; set; }

        public Root()
        {
            Version = 1;
            LogDir = string.Empty;
            Program = new Program();
        }
    }

    public class Klean
    {
        static void Main()
        {
            string settingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (!File.Exists(settingsPath))
                ConsoleError($"{settingsPath} doesn't exist!");

            string json = File.ReadAllText(settingsPath);
            Root? Settings = JsonSerializer.Deserialize<Root>(document: JsonDocument.Parse(json));

            if (Settings == null)
                ConsoleError("Error deserializing settings.json!");

            if (Settings?.Version > 1)
                ConsoleError("Error in settings.json: 'v' is invalid!");

            string LogFile = "ProcessTerminationLog.txt";

            string[] NoKil = {
                "cmd", "explorer", "panik", "taskmgr", "fences", "dwm", "devenv", "svchost", "conhost",
                "ctfmon", "sihost", "nvcontainer", "Microsoft.ServiceHub.Controller", "dllhost",
                "StartMenuExperienceHost", "ServiceHub.IdentityHost", "ServiceHub.VSDetouredHost",
                "SearchHost", "RuntimeBroker", "Widgets", "dllhost", "WmiPrvSE", "NvOAWrapperCache",
                "ServiceHub.Host.dotnet.x64", "TextInputHost", "MSBuild", "VBCSCompiler",
                "VsDebugConsole"
            };

            if (Settings?.Program?.Apps != null)
            {
                foreach (var app in Settings.Program.Apps)
                {
                    if (!NoKil.Contains(app.Name?.ToLower()))
                        NoKil = [.. NoKil, app.Name?.ToLower()];
                }
            }

            Process[] processes = Process.GetProcesses();
            using (StreamWriter writer = new StreamWriter(LogFile, append: false))
            {
                foreach (Process process in processes)
                {
                    string processName = process.ProcessName.ToLower();
                    if (!NoKil.Any(excluded => processName.Equals(excluded.ToLower())))
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
            Console.WriteLine($"Process termination complete. Log written to: {LogFile}");
            Console.ReadKey();
        }

        private static void ConsoleError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(0);
        }
    }
}
