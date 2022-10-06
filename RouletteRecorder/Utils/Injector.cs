using RouletteRecorder.Monitors;
using System;
using System.Diagnostics;
using System.IO;

namespace RouletteRecorder.Utils
{
    public class Injector
    {
        Process cmd;

        private void MessageHandler(object sender, DataReceivedEventArgs args) => GameMonitor.HandleMessage(args.Data);

        public void Init()
        {
            Process FFXIV = Helper.GetFFXIVProcess();
            string exePath = Path.Combine(Helper.GetPluginDir(), "RouletteRecorderConsole.exe");
            cmd = new Process();
#if (!DEBUG)
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif
            cmd.StartInfo.FileName = exePath;
            cmd.StartInfo.Arguments = FFXIV.Id.ToString();
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.OutputDataReceived += MessageHandler;
        }

        public void Start()
        {
            cmd.Start();
            cmd.BeginOutputReadLine();
        }

        public void Stop()
        {
            cmd.OutputDataReceived -= MessageHandler;
            cmd.Kill();
            cmd = null;
        }
    }
}
