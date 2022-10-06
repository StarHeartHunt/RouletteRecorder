namespace RouletteRecorder.Utils
{
    internal class Log
    {
        public delegate void EventHandler(char type, string message);
        public static event EventHandler Handler;
        public static void Add(char type, string message)
        {
            Handler?.Invoke(type, message);
        }

        public static void Error(string message)
        {
            Add('E', message);
        }

        public static void Warn(string message)
        {
            Add('W', message);
        }

        public static void Info(string message)
        {
            Add('I', message);
        }

#if DEBUG
        public static void Debug(string message)
        {
            Add('D', message);
        }
#endif
    }
}
