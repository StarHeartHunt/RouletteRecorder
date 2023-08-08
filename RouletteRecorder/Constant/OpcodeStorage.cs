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
            { 0x0223, Opcode.ActorControlSelf },
            { 0x01EB, Opcode.ContentFinderNotifyPop },
            { 0x018E, Opcode.InitZone },
        };
    }
}
