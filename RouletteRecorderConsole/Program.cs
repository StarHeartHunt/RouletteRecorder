using EasyHook;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace RouletteRecorderConsole
{
    internal class Program
    {
        static String channelName = null;
        private static Process FFXIV;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                FFXIV = Process.GetProcessesByName("ffxiv_dx11").FirstOrDefault();
            }
            else
            {
                FFXIV = Process.GetProcessById(int.Parse(args[0]));
            }
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string dllPath = Path.Combine(Path.GetDirectoryName(exePath), "RouletteRecorderPayload.dll");
            string dllDir = (Config.DependencyPath = (Config.HelperLibraryLocation = Path.GetDirectoryName(dllPath)));

            RemoteHooking.IpcCreateServer<ServerInterface>(ref channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton);
            RemoteHooking.Inject(FFXIV.Id, InjectionOptions.DoNotRequireStrongName, dllPath, dllPath, channelName);

            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}
