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
            { 0x01A3, Opcode.ActorControlSelf },
            { 0x00CA, Opcode.ContentFinderNotifyPop },
            { 0x01E3, Opcode.InitZone },
        };
    }
}
