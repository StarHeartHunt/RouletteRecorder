namespace RouletteRecorder.Constant
{
    public enum LogType
    {
        None,
        LogLine,
        State,
        Event,
        Database,

#if DEBUG
        Request,
        ActorControlSelf,
        InvalidPacket,
        RawPacket,
        Debug1,
#endif
    }
}
