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
        [JsonProperty("_key")] string Key,
        [JsonProperty("_name")] string Name,
        [JsonProperty("_isforced")] bool IsForced
    );

    public record Program(
        [JsonProperty("app")] IReadOnlyList<App> Apps
    );

    public record Root(
        [JsonProperty("v")] int Version,
        [JsonProperty("logdir")] string LogDir,
        [JsonProperty("program")] Program Program
    );

    public class Klean
    {
        static void Main()
        {
            string Settings = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (!File.Exists(Settings))
                ThrwCslErr($"{Settings} doesn't exist!");

            string Json = File.ReadAllText(Settings);

            Root ProgramSettings = JsonConvert.DeserializeObject<Root>(Json);

            if (ProgramSettings?.Version == null)
                ThrwCslErr($"Error in settings.json: 'v' is null!");
            if (ProgramSettings?.Version is > 1 or 0)
                ThrwCslErr("Invalid settings version");

            string LogFil = "ProcessTerminationLog.txt";

            string[] NoKil = {
                "cmd", "explorer", "panik", "taskmgr", "fences", "dwm", "devenv", "svchost", "conhost",
                "ctfmon", "sihost", "nvcontainer", "Microsoft.ServiceHub.Controller", "dllhost",
                "StartMenuExperienceHost", "ServiceHub.IdentityHost", "ServiceHub.VSDetouredHost",
                "SearchHost", "RuntimeBroker", "Widgets", "dllhost", "WmiPrvSE", "NvOAWrapperCache",
                "ServiceHub.Host.dotnet.x64", "TextInputHost", "MSBuild", "VBCSCompiler",
                "VsDebugConsole"
            };


            if (ProgramSettings?.Program?.Apps != null)
            {
                foreach (var App in ProgramSettings.Program.Apps)
                {
                    if (!NoKil.Contains(App.Name.ToLower()))
                        NoKil = NoKil.Append(App.Name.ToLower()).ToArray();
                }
            }

            Process[] Processes = Process.GetProcesses();
            using (StreamWriter Writer = new StreamWriter(LogFil, append: false))
            {
                foreach (Process Process in Processes)
                {
                    string ProcessName = Process.ProcessName.ToLower();
                    if (!NoKil.Any(Excluded => ProcessName.Equals(Excluded.ToLower())))
                    {
                        try
                        {
                            Kalm.Success(Writer, Process);
                        }
                        catch (Exception Ex)
                        {
                            UhOh.Fail(Writer, Process, Ex);
                        }
                    }
                }
            }
            Console.WriteLine($"Process termination complete. Log written to: {LogFil}");
            Console.ReadKey();
        }

        private static void ThrwCslErr(string ErrorMessage)
        {
            Console.WriteLine(ErrorMessage);
            Environment.Exit(0);
        }

        //panik
    }
}
