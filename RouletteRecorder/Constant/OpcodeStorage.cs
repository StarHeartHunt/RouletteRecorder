using System.Collections.Generic;

namespace RouletteRecorder.Constant
{
    internal enum Opcode
    {
        ActorControl,
        ActorControlSelf,
        CEDirector,
        CompanyAirshipStatus,
        CompanySubmersibleStatus,
        ContentFinderNotifyPop,
        DirectorStart,
        EventPlay,
        Examine,
        FateInfo,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
    }
    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, Opcode> China = new Dictionary<ushort, Opcode>
        {
            { 0x00e4, Opcode.ActorControl },
            { 0x0125, Opcode.ActorControlSelf },
            { 0x019a, Opcode.CEDirector },
            { 0x01a5, Opcode.CompanyAirshipStatus },
            { 0x0112, Opcode.CompanySubmersibleStatus },
            { 0x021b, Opcode.ContentFinderNotifyPop },
            { 0x017b, Opcode.DirectorStart },
            { 0x0101, Opcode.EventPlay },
            { 0x0382, Opcode.Examine },
            { 0x007a, Opcode.FateInfo },
            { 0x026f, Opcode.InitZone },
            { 0x01b9, Opcode.InventoryTransaction },
            { 0x0255, Opcode.ItemInfo },
            { 0x0168, Opcode.MarketBoardItemListing },
            { 0x01ac, Opcode.MarketBoardItemListingCount },
            { 0x0254, Opcode.MarketBoardItemListingHistory },
            { 0x0349, Opcode.NpcSpawn },
            { 0x036d, Opcode.PlayerSetup },
            { 0x0220, Opcode.PlayerSpawn },
        };
    }
}
