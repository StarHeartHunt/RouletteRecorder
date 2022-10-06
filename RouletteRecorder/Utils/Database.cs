using RouletteRecorder.DAO;
using System.IO;
using System.Text;

namespace RouletteRecorder.Utils
{
    public static class Database
    {
        private static string header = "任务类型,日期,开始时间,结束时间,副本名称,职业,完成情况";
        private static string path = Helper.GetDbPath();
        private static FileStream fs;
        public static bool InitDatabase()
        {
            if (fs == null)
            {
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            }

            using (StreamReader streamReader = new StreamReader(fs, Encoding.UTF8, true, 4096, true))
            {
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string line = streamReader.ReadLine();
                if (line == null)
                {
                    using (StreamWriter streamWriter = new StreamWriter(fs, Encoding.UTF8, 4096, true))
                    {
                        streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                        streamWriter.WriteLine(header);
                    }
                }
            }
            return true;
        }
        public static bool InsertRoulette(Roulette roulette)
        {
            var line = string.Format(
                "{0},{1},{2},{3},{4},{5},{6}",
                roulette.RouletteType,
                roulette.Date,
                roulette.StartedAt,
                roulette.EndedAt,
                roulette.RouletteName,
                roulette.JobName,
                roulette.IsCompleted
                );
            using (StreamWriter streamWriter = new StreamWriter(fs, Encoding.UTF8, 1024, true))
            {
                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.WriteLine(line);
            }
            return true;
        }

        public static bool Release()
        {
            fs.Flush(true);
            fs.Dispose();

            return true;
        }
    }
}
