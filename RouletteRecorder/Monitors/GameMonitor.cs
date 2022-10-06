using RouletteRecorder.Utils;
using System;

namespace RouletteRecorder.Monitors
{
    public class GameMonitor
    {
        public enum OpCode
        {
            Ping,
            IsInstalled,
            SetGlobalBgm,
            Exception
        }
        public static void HandleMessage(string message)
        {
            try
            {
                if (message == null) return;
                string[] array = message.Split(':');
                OpCode opcode = (OpCode)int.Parse(array[0]);
                HandleMessageReceived(message, array, opcode);
            }
            catch (Exception ex)
            {
                Log.Error("Received message:" + message);
                Log.Error(ex.ToString());
                return;
            }
        }

        public static void HandleMessageReceived(string message, string[] array, OpCode opcode)
        {
            if (array == null)
            {
                return;
            }
            switch (opcode)
            {
                case OpCode.IsInstalled:
                    {
                        Log.Info("Payload installed:" + message);
                        break;
                    }

                case OpCode.Exception:
                    {
                        Log.Info("Received Exception:" + message);
                        break;
                    }

                case OpCode.SetGlobalBgm:
                    {
                        if (int.Parse(array[1]) == 18)
                        {
                            Log.Info("[GameMonitor] Detected Victory BGM:" + message);
                            RouletteSingleton.Instance.IsCompleted = true;
                            if (RouletteSingleton.Instance.RouletteType != null)
                            {
                                RouletteSingleton.Instance.Finish();
                            }
                        }
                        break;
                    }
            }
        }
    }
}
