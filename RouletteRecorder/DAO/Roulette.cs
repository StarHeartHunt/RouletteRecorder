using Advanced_Combat_Tracker;
using CsvHelper.Configuration.Attributes;
using RouletteRecorder.Utils;
using System;
using System.Linq;

namespace RouletteRecorder.DAO
{
    public class Roulette
    {
        public static Roulette Instance { get; private set; } = null;

        public static void Init(string rouletteName = null, string rouletteType = null, bool isCompleted = false)
        {
            Instance = new Roulette(rouletteName, rouletteType, isCompleted);
        }

        [Name("日期")]
        public string Date { get; set; }

        [Name("开始时间")]
        public string StartedAt { get; set; }

        [Name("结束时间")]
        public string EndedAt { get; set; }

        [Name("副本名称")]
        public string RouletteName { get; set; }

        [Name("任务类型")]
        public string RouletteType { get; set; }

        [Name("职业")]
        public string JobName { get; set; }

        [Name("完成情况")]
        public bool IsCompleted { get; set; }

        public Roulette(string rouletteName = null, string rouletteType = null, bool isCompleted = false)
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            StartedAt = DateTime.Now.ToString("T");
            EndedAt = "";
            RouletteName = rouletteName;
            RouletteType = rouletteType;
            IsCompleted = isCompleted;
        }
        public void Finish()
        {
            var ffxivPlugin = (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)Helper.GetFFXIVPlugin();
            var jobId = ffxivPlugin.DataRepository.GetPlayer().JobID;
            if (Data.Instance.Jobs.TryGetValue(Convert.ToInt32(jobId), out var jobName))
            {
                Instance.JobName = jobName.Name;
            }
            else
            {
                Instance.JobName = "未知职业";
            }
            Instance.EndedAt = DateTime.Now.ToString("T");
            if (
                Instance.RouletteType != null
                && Config.Instance.RouletteTypes.Select(type => Data.Instance.Roulettes[type].Chinese).Contains(Instance.RouletteType)
            )
            {
                Database.InsertRoulette(Instance);
            }
        }
    }
}
