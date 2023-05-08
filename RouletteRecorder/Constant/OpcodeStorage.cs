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
            { 0x0139, Opcode.ActorControlSelf },
            { 0x019D, Opcode.ContentFinderNotifyPop },
            { 0x01C5, Opcode.InitZone },
        };
    }
}
