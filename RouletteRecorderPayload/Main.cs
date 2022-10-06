using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;

namespace RouletteRecorderPayload
{
    public class Main : IEntryPoint
    {
        RouletteRecorderConsole.ServerInterface Interface;
        LocalHook SetGlobalBgmHook;
        public IntPtr SetGlobalBgm { get; private set; }
        private delegate long SetGlobalBgmDelegate(ushort bgmKey, byte a2, uint a3, uint a4, uint a5, byte a6);
        private SetGlobalBgmDelegate SetGlobalBgmOriginal;
        private Process FFXIV;
        private SigScanner scanner;

        public Main(RemoteHooking.IContext InContext, string ChannelName)
        {
            FFXIV = Process.GetCurrentProcess();
            scanner = new SigScanner(FFXIV.MainModule);
            Interface = RemoteHooking.IpcConnectClient<RouletteRecorderConsole.ServerInterface>(ChannelName);
            Interface.Ping();
        }

        public void Run(RemoteHooking.IContext InContext, string ChannelName)
        {
            try
            {
                SetGlobalBgm = scanner.ScanText("4C 8B 15 ?? ?? ?? ?? 4D 85 D2 74 58");
                SetGlobalBgmHook = LocalHook.Create(SetGlobalBgm, new SetGlobalBgmDelegate(SetGlobalBgmDetour), null);
                SetGlobalBgmOriginal = Marshal.GetDelegateForFunctionPointer<SetGlobalBgmDelegate>(SetGlobalBgmHook.HookBypassAddress);
                SetGlobalBgmHook.ThreadACL.SetExclusiveACL(null);
            }
            catch (Exception ex)
            {
                Interface.ReportException(ex);
                return;
            }
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            try
            {
                while (true)
                {
                    Thread.Sleep(500);
                    Interface.Ping();
                }
            }
            catch
            {
            }
        }
        public long SetGlobalBgmDetour(ushort bgmKey, byte a2, uint a3, uint a4, uint a5, byte a6)
        {
            try
            {
                Interface.OnSetGlobalBgm(bgmKey, a2, a3, a4, a5, a6);
            }
            catch (Exception ex)
            {
                Interface.ReportException(ex);
            }
            return SetGlobalBgmOriginal(bgmKey, a2, a3, a4, a5, a6);
        }

    }

}
