using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Panik.Klass;

namespace Panik
{
    public record App(
        [JsonProperty("_key")] string _key,
        [JsonProperty("_name")] string _name,
        [JsonProperty("_isforced")] bool _isforced
    );

    public record Program(
        [JsonProperty("app")] IReadOnlyList<App> app
    );

    public record Root(
        [JsonProperty("v")] int v,
        [JsonProperty("logdir")] string logdir,
        [JsonProperty("program")] Program program
    );

    public class Klean
    {
        static void Main()
        {
            string AppSettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (!File.Exists(AppSettingsPath))
                ThrwCslErr($"{AppSettingsPath} doesn't exist!");

            string json = File.ReadAllText(AppSettingsPath);

            Root ProgramSettings = JsonConvert.DeserializeObject<Root>(json);

            if (ProgramSettings?.v == null)
                ThrwCslErr($"Error in settings.json: 'v' is null!");
            if (ProgramSettings?.v is > 1 or 0)
                ThrwCslErr("Invalid settings version");

            string LogFile = "ProcessTerminationLog.txt";

            string[] NoKill = {
                "cmd", "explorer", "panik", "taskmgr", "fences", "dwm", "devenv", "svchost", "conhost",
                "ctfmon", "sihost", "nvcontainer", "Microsoft.ServiceHub.Controller", "dllhost",
                "StartMenuExperienceHost", "ServiceHub.IdentityHost", "ServiceHub.VSDetouredHost",
                "SearchHost", "RuntimeBroker", "Widgets", "dllhost", "WmiPrvSE", "NvOAWrapperCache",
                "ServiceHub.Host.dotnet.x64", "TextInputHost", "MSBuild", "VBCSCompiler",
                "VsDebugConsole"
            };


            if (ProgramSettings.program?.app != null)
            {
                foreach (var app in ProgramSettings.program.app)
                {
                    if (!NoKill.Contains(app._name.ToLower()))
                        NoKill = NoKill.Append(app._name.ToLower()).ToArray();
                }
            }

            Process[] Processes = Process.GetProcesses();
            using (StreamWriter writer = new StreamWriter(LogFile, append: false))
            {
                foreach (Process process in Processes)
                {
                    string processName = process.ProcessName.ToLower();
                    if (!NoKill.Any(excluded => processName.Equals(excluded.ToLower())))
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

        private static void ThrwCslErr(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(0);
        }

        //panik
    }
}
