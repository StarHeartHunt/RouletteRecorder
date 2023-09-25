using CsvHelper.Configuration.Attributes;
using System;

namespace RouletteRecorder.DAO
{
    public class Roulette
    {
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

        public void Init(string rouletteName = null, string rouletteType = null, bool isCompleted = false)
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            StartedAt = DateTime.Now.ToString("T");
            EndedAt = "";
            RouletteName = rouletteName;
            RouletteType = rouletteType;
            IsCompleted = isCompleted;
        }
    }
}
