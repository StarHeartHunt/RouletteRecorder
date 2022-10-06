using System;
using System.Text;
using RouletteRecorder.Monitors;

namespace RouletteRecorderConsole
{
    public class ServerInterface : MarshalByRefObject
    {

        public void IsInstalled(int clientPID)
        {
            Console.WriteLine("{0}:{1}", (int)GameMonitor.OpCode.IsInstalled, clientPID);
        }

        public void OnSetGlobalBgm(ushort bgmKey, byte a2, uint a3, uint a4, uint a5, byte a6)
        {
            Console.WriteLine(
                "{0}:{1}:{2}:{3}:{4}:{5}:{6}",
                (int)GameMonitor.OpCode.SetGlobalBgm,
                bgmKey, a2, a3, a4, a5, a6
                );
        }

        public void ReportException(Exception ex)
        {
            Console.WriteLine("{0}:{1}", (int)GameMonitor.OpCode.Exception, Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.ToString())));
        }

        public void Ping()
        {
            Console.WriteLine("{0}:0", (int)GameMonitor.OpCode.Ping);
        }
    }
}
