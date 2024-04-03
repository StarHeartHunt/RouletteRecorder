using RouletteRecorder.Constant;
using RouletteRecorder.DAO;
using RouletteRecorder.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteRecorder.Monitors
{
    public class NetworkMonitor
    {
        private static bool ToInternalOpcode(ushort opcode, out Opcode internalOpcode)
        {
            var region = Config.Instance.Region;
            switch (region)
            {
                case Region.Global:
                    return OpcodeStorage.Global.TryGetValue(opcode, out internalOpcode);

                case Region.China:
                    return OpcodeStorage.China.TryGetValue(opcode, out internalOpcode);

                default:
                    internalOpcode = default;
                    return false;
            }
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
                    FireException(e);
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

            var processed = HandleMessageByOpcode(message);
            if (processed) return;
#if DEBUG
            if (!ToInternalOpcode(BitConverter.ToUInt16(message, 18), out var opcode)) return;
            LogIncorrectPacketSize(opcode, message.Length);
            Log.Packet(message);
#endif
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
            // Log.Debug(LogType.Event, $"[NetworkMonitor] source:{source}, target:{target}, data:{data}");

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
                Roulette.Instance.RouletteName = Data.Instance.Instances.TryGetValue(contentId, out var instanceData)
                                                    ? instanceData.Name.ToString()
                                                    : "未知副本";
                Log.Info(LogType.State, "[NetworkMonitor] Detected InitZone: serverId:{serverId}, zoneId:{zoneId}, instanceId:{instanceId}, contentId:{contentId}");
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
                        Log.Info(LogType.State, "[NetworkMonitor] Detected ActorControlSelf (Victory)");

                        Roulette.Instance.IsCompleted = true;
                        if (Roulette.Instance.RouletteType != null)
                        {
                            Task.Run(() => Roulette.Instance.Finish());
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
                Log.Info(Constant.LogType.State, $"[NetworkMonitor] Detected ContentFinderNotifyPop: roulette:{roulette}, instance:{instance}");

                if (roulette == 0) return false;

                Roulette.Instance?.Finish();
                Roulette.Init();
                if (Roulette.Instance != null)
                {
                    Roulette.Instance.RouletteType = Data.Instance.Roulettes.TryGetValue(roulette, out var rouletteName)
                        ? rouletteName.ToString()
                        : "未知随机任务";
                }
            }

            return true;
        }

        public delegate void ExceptionHandler(Exception e);
        public event ExceptionHandler OnException;
        private void FireException(Exception e)
        {
            OnException?.Invoke(e);
        }

        public void HandleMessageSent(string connection, long epoch, byte[] message)
        {
        }

#if DEBUG
        private void LogIncorrectPacketSize(Opcode opcode, int size)
        {
            Log.Warn(LogType.InvalidPacket, $"{Enum.GetName(typeof(Opcode), opcode)} length {size}");
        }
#endif
    }
}
