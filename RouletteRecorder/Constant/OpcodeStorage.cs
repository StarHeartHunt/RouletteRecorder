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
        public static Dictionary<ushort, Opcode> China = new Dictionary<ushort, Opcode>
        {
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.ActorControlSelf, Opcode.ActorControlSelf },
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.ContentFinderNotifyPop, Opcode.ContentFinderNotifyPop },
            { (ushort)FFXIVOpcodes.CN.ServerZoneIpcType.InitZone, Opcode.InitZone },
        };
    }
}
