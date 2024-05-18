using CsvHelper;
using CsvHelper.Configuration;
using RouletteRecorder.DAO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace RouletteRecorder.Utils
{
    public static class Database
    {
        private static readonly string path = Helper.GetDbPath();

        private static readonly FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

        public static bool InitDatabase()
        {
            if (new FileInfo(path).Length > 0) return true;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine
            };

            using (var writer = new StreamWriter(fs, Encoding.UTF8, 4096, true))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(new List<Roulette>());
            }

            return true;
        }

        public static bool InsertRoulette(Roulette roulette)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = false,
            };

            using (var writer = new StreamWriter(fs, Encoding.UTF8, 4096, true))
            using (var csv = new CsvWriter(writer, config))
            {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                csv.WriteRecords(new List<Roulette>() { roulette });
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
