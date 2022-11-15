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
            { 0x01d4, Opcode.ActorControl },
            { 0x0125, Opcode.ActorControlSelf },
            { 0x00d1, Opcode.CEDirector },
            { 0x0288, Opcode.CompanyAirshipStatus },
            { 0x032d, Opcode.CompanySubmersibleStatus },
            { 0x021b, Opcode.ContentFinderNotifyPop },
            { 0x013b, Opcode.DirectorStart },
            { 0x0282, Opcode.EventPlay },
            { 0x03e0, Opcode.Examine },
            { 0x0237, Opcode.FateInfo },
            { 0x026f, Opcode.InitZone },
            { 0x0244, Opcode.InventoryTransaction },
            { 0x02d3, Opcode.ItemInfo },
            { 0x00fd, Opcode.MarketBoardItemListing },
            { 0x0349, Opcode.MarketBoardItemListingCount },
            { 0x01d7, Opcode.MarketBoardItemListingHistory },
            { 0x015d, Opcode.NpcSpawn },
            { 0x0205, Opcode.PlayerSetup },
            { 0x0068, Opcode.PlayerSpawn },
        };
    }
}
