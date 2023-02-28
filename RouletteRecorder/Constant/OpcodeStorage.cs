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
            { 0x00F1, Opcode.ActorControlSelf },
            { 0x02DD, Opcode.ContentFinderNotifyPop },
            { 0x00CE, Opcode.InitZone },
        };
    }
}
