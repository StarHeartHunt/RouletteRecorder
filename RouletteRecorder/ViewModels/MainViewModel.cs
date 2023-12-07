using FFXIV_ACT_Plugin.Common;
using RouletteRecorder.Constant;
using RouletteRecorder.Utils;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace RouletteRecorder.ViewModels
{
    public class MonitorTypeNode : BindingTarget
    {
        public MonitorTypeNode(Monitors.MonitorType type, string name, bool isSelected)
        {
            Type = type;
            Name = name;
            IsSelected = isSelected;
        }
        public Monitors.MonitorType Type { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
    }

    internal class MonitorTypeNodeList
    {
        public static ListBindingTarget<MonitorTypeNode> Create(Dictionary<Monitors.MonitorType, string> monitors)
        {
            var result = new ListBindingTarget<MonitorTypeNode>();
            if (monitors == null) return result;
            foreach (var pair in monitors)
            {
                result.Add(new MonitorTypeNode(pair.Key, pair.Value, pair.Key == Monitors.MonitorType.Network));
            }
            return result;
        }
    }

    public class RouletteTypeNode : BindingTarget
    {
        public RouletteTypeNode(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public bool IsChecked { get; set; } = false;

        public string Name { get; set; }
    }

    internal class RouletteTypeNodeList
    {
        public static ListBindingTarget<RouletteTypeNode> Create(Dictionary<int, Models.ItemName> rouletteTypes)
        {
            var result = new ListBindingTarget<RouletteTypeNode>();
            if (rouletteTypes == null) return result;
            foreach (var pair in rouletteTypes)
            {
                result.Add(new RouletteTypeNode(pair.Key, pair.Value.Chinese));
            }
            return result;
        }
    }

    public struct L12nRegion
    {
        public Constant.Region ID { get; set; }
        public Models.ItemName Name { get; set; }
    }

    public struct L12nLanguage
    {
        public Language ID { get; set; }
        public string Name { get; set; }
    }

    public class MainViewModel : BindingTarget
    {
        public MainViewModel()
        {
            Config.Instance.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Language")
                {
                    var ne = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    Regions.EmitCollectionChanged(ne);
                }
            };
        }

        public ListBindingTarget<L12nLanguage> Languages { get; } = new ListBindingTarget<L12nLanguage>
        {
            new L12nLanguage
            {
                ID = Language.English,
                Name = "English"
            },
            new L12nLanguage
            {
                ID = Language.French,
                Name = "Français"
            },
            new L12nLanguage
            {
                ID = Language.German,
                Name = "Deutsche"
            },
            new L12nLanguage
            {
                ID = Language.Japanese,
                Name = "日本語"
            },
            new L12nLanguage
            {
                ID = Language.Chinese,
                Name = "中文"
            },
        };

        public ListBindingTarget<L12nRegion> Regions { get; } = new ListBindingTarget<L12nRegion>
        {
            new L12nRegion
            {
                ID = Constant.Region.Global,
                Name = new Models.ItemName
                {
                    English = "Global",
                    Chinese = "国际服"
                }
            },
            new L12nRegion
            {
                ID = Constant.Region.China,
                Name = new Models.ItemName
                {
                    English = "China",
                    Chinese = "国服"
                }
            },
        };

        public ListBindingTarget<MonitorTypeNode> Monitors { get; set; } = MonitorTypeNodeList.Create(Config.MonitorTypes);
        public int SelectedMonitorIndex { get; set; } = 0;
        public ListBindingTarget<RouletteTypeNode> RouletteTypes { get; set; } = RouletteTypeNodeList.Create(null);

        // Logging
        public string Log { get; set; } = "";
        public bool LogPause { get; set; } = false;
        public bool LogTypeFilter { get; set; } = false;
        public LogType LogTypeFilterValue { get; set; }

        public uint LogShowCount { get; set; } = 0;
        public uint LogAllCount { get; set; } = 0;

        public string LogStatus
        {
            get
            {
                return $"({LogShowCount}/{LogAllCount})";
            }
        }

        public string PauseText
        {
            get
            {
                return LogPause ? "恢复" : "暂停";
            }
        }
    }
}
