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
            { 0x03D5, Opcode.ActorControlSelf },
            { 0x00EF, Opcode.ContentFinderNotifyPop },
            { 0x00BC, Opcode.InitZone },
        };
    }
}
