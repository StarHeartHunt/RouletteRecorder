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
            { 0x0397, Opcode.ActorControlSelf },
            { 0x00CF, Opcode.ContentFinderNotifyPop },
            { 0x008B, Opcode.InitZone },
        };
    }
}
