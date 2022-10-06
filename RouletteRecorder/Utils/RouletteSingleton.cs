using RouletteRecorder.DAO;
using System;
using System.Linq;

namespace RouletteRecorder.Utils
{
    public class RouletteSingleton : Roulette
    {
        private static readonly Lazy<RouletteSingleton> lazy =
           new Lazy<RouletteSingleton>(() => new RouletteSingleton());
        public static RouletteSingleton Instance { get { return lazy.Value; } }

        private RouletteSingleton()
        {
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
