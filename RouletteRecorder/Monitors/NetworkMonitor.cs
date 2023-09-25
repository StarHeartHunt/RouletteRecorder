using RouletteRecorder.Constant;
using RouletteRecorder.DAO;
using RouletteRecorder.Utils;
using System;
using System.Linq;

namespace RouletteRecorder.Monitors
{
    public class NetworkMonitor
    {
        private bool ToInternalOpcode(ushort opcode, out Opcode internalOpcode)
        {
            return OpcodeStorage.China.TryGetValue(opcode, out internalOpcode);
        }

        public void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            try
            {
                HandleMessage(message);
            }
            catch (Exception e)
            {
                try
                {
                    Log.Error(string.Format("[{0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
                }
                catch { }
            }
        }

        private void HandleMessage(byte[] message)
        {
            var segmentType = message[12];
            // Deucalion gives wrong type (0)
            if (message.Length < 32 || (segmentType != 0 && segmentType != 3))
            {
                return;
            }

            HandleMessageByOpcode(message);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private bool HandleMessageByOpcode(byte[] message)
        {
            if (!ToInternalOpcode(BitConverter.ToUInt16(message, 18), out var opcode))
            {
                return false;
            }

            var source = BitConverter.ToUInt32(message, 4);
            var target = BitConverter.ToUInt32(message, 8);
            var data = message.Skip(32).ToArray();
            // Log.Debug($"[NetworkMonitor] source:{source}, target:{target}, data:{data}");

            if (opcode == Opcode.InitZone)
            {
                if (message.Length != 136)
                {
                    return false;
                }

                var serverId = BitConverter.ToUInt16(data, 0);
                var zoneId = BitConverter.ToUInt16(data, 2);
                var instanceId = BitConverter.ToUInt16(data, 4);
                var contentId = BitConverter.ToUInt16(data, 6);

                if (Roulette.Instance == null)
                {
                    Roulette.Init();
                }

                if (Data.Instance.Instances.TryGetValue(contentId, out var instanceData))
                {
                    Roulette.Instance.RouletteName = instanceData.Name.ToString();
                }
                else
                {
                    Roulette.Instance.RouletteName = "未知副本";
                }
                Log.Info($"[NetworkMonitor] Detected InitZone: serverId:{serverId}, zoneId:{zoneId}, instanceId:{instanceId}, contentId:{contentId}");
            }
            else if (opcode == Opcode.ActorControlSelf)
            {
                var category = BitConverter.ToUInt16(data, 0);
                if (category == 109) // DirectorUpdate
                {
                    var param2 = data.Skip(8).ToArray(); //struct FFXIVIpcActorControlTarget : FFXIVIpcBasePacket< ActorControlTarget >
                    if (
                        BitConverter.ToInt32(param2, 0) == 0x40000003
                        || BitConverter.ToInt32(param2, 0) == 0x40000002) // Victory: 21:zone:40000003:00:00:00:00 行会令 40000002
                    {
                        Log.Info($"[NetworkMonitor] Detected ActorControlSelf (Victory)");
                        if (Config.Instance.MonitorType == MonitorType.Network)
                        {
                            Roulette.Instance.IsCompleted = true;
                            if (Roulette.Instance.RouletteType != null)
                            {
                                Roulette.Instance.Finish();
                            }
                        }
                    }
                }
            }
            else if (opcode == Opcode.ContentFinderNotifyPop)
            {
                if (message.Length != 72)
                {
                    return false;
                }
                var roulette = BitConverter.ToUInt16(data, 2);
                var instance = roulette == 0 ? BitConverter.ToUInt16(data, 0x1c) : 0;
                Log.Info(string.Format("[NetworkMonitor] Detected ContentFinderNotifyPop: roulette:{0}, instance:{1}", roulette, instance));
                Roulette.Init();
                if (roulette == 0) return false;
                if (Data.Instance.Roulettes.TryGetValue(roulette, out var rouletteName))
                {
                    Roulette.Instance.RouletteType = rouletteName.ToString();
                }
                else
                {
                    Roulette.Instance.RouletteType = "未知随机任务";
                }

            }

            return true;
        }

        public void HandleMessageSent(string connection, long epoch, byte[] message)
        {
        }
    }
}
