using System.Diagnostics;

namespace Panik
{
    public class Klean
    {
        static void Main()
        {
            string LogFil = "ProcessTerminationLog.txt";
            string[] NoKil = {
            "cmd", "explorer", "taskmgr","panik", "proton",
            "proton vpn", "NLClientApp", "FL64", "xampp",
            "ProcessLasso", "vpn", "tailscale", "chrome",
            "firefox", "5795795798567957", "fences", "dwm", "discord",
            "devenv", "svchost", "brave", "conhost",
            "ctfmon", "sihost", "nvcontainer", "Microsoft.ServiceHub.Controller", "dllhost",
            "StartMenuExperienceHost", "ServiceHub.IdentityHost", "ServiceHub.VSDetouredHost",
            "SearchHost", "RuntimeBroker", "Widgets", "dllhost", "WmiPrvSE", "NvOAWrapperCache",
            "ServiceHub.Host.dotnet.x64", "TextInputHost", "MSBuild", "VBCSCompiler", "VsDebugConsole"
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
        }
        //panik
    }
}

