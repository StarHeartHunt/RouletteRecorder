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
            { 0x0365, Opcode.ActorControl },
            { 0x0245, Opcode.ActorControlSelf },
            { 0x008C, Opcode.CEDirector },
            { 0x0272, Opcode.CompanyAirshipStatus },
            { 0x026E, Opcode.CompanySubmersibleStatus },
            { 0x0171, Opcode.ContentFinderNotifyPop },
            { 0x017b, Opcode.DirectorStart },
            { 0x0067, Opcode.EventPlay },
            { 0x00ED, Opcode.Examine },
            { 0x03D2, Opcode.FateInfo },
            { 0x0356, Opcode.InitZone },
            { 0x01b9, Opcode.InventoryTransaction },
            { 0x0255, Opcode.ItemInfo },
            { 0x0069, Opcode.MarketBoardItemListing },
            { 0x007E, Opcode.MarketBoardItemListingCount },
            { 0x0141, Opcode.MarketBoardItemListingHistory },
            { 0x011D, Opcode.NpcSpawn },
            { 0x0217, Opcode.PlayerSetup },
            { 0x02B6, Opcode.PlayerSpawn },
        };
    }
}
