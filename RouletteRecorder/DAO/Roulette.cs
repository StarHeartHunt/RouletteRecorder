using System;

namespace RouletteRecorder.DAO
{
    public class Roulette
    {
        public string Date { get; set; }
        public string StartedAt { get; set; }
        public string EndedAt { get; set; }
        public string RouletteName { get; set; }
        public string RouletteType { get; set; }
        public string JobName { get; set; }
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
