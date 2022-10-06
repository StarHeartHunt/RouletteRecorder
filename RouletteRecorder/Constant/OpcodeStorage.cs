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
            { 0x012d, Opcode.ActorControl },
            { 0x02b4, Opcode.ActorControlSelf },
            { 0x0356, Opcode.CEDirector },
            { 0x01e7, Opcode.CompanyAirshipStatus },
            { 0x0101, Opcode.CompanySubmersibleStatus },
            { 0x03d1, Opcode.ContentFinderNotifyPop },
            { 0x011a, Opcode.DirectorStart },
            { 0x0321, Opcode.EventPlay },
            { 0x01c7, Opcode.Examine },
            { 0x01b5, Opcode.FateInfo },
            { 0x019b, Opcode.InitZone },
            { 0x0281, Opcode.InventoryTransaction },
            { 0x03bc, Opcode.ItemInfo },
            { 0x0190, Opcode.MarketBoardItemListing },
            { 0x00ef, Opcode.MarketBoardItemListingCount },
            { 0x0351, Opcode.MarketBoardItemListingHistory },
            { 0x03cb, Opcode.NpcSpawn },
            { 0x00a7, Opcode.PlayerSetup },
            { 0x02c6, Opcode.PlayerSpawn },
        };
    }
}
