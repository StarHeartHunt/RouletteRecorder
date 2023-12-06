using System.Collections.Generic;

namespace RouletteRecorder.Constant
{
    internal enum Opcode
    {
        ActorControlSelf,
        ContentFinderNotifyPop,
        InitZone,
    }
    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, Opcode> Global = new Dictionary<ushort, Opcode>
        {
            { (ushort)FFXIVOpcodes.Global.ServerZoneIpcType.ActorControlSelf, Opcode.ActorControlSelf },
            { (ushort)FFXIVOpcodes.Global.ServerZoneIpcType.ContentFinderNotifyPop, Opcode.ContentFinderNotifyPop },
            { (ushort)FFXIVOpcodes.Global.ServerZoneIpcType.InitZone, Opcode.InitZone },
        };

        public static Dictionary<ushort, Opcode> China = new Dictionary<ushort, Opcode>
        {
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.ActorControlSelf, Opcode.ActorControlSelf },
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.ContentFinderNotifyPop, Opcode.ContentFinderNotifyPop },
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.InitZone, Opcode.InitZone },
        };
    }
}
