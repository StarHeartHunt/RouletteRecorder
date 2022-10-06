using RouletteRecorder.Utils;
using System.Collections.Generic;

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

    public class MainViewModel : BindingTarget
    {
        public MainViewModel()
        {
        }

        public string Log { get; set; } = "";

        public ListBindingTarget<MonitorTypeNode> Monitors { get; set; } = MonitorTypeNodeList.Create(Config.MonitorTypes);

        public int SelectedMonitorIndex { get; set; } = 0;

        public ListBindingTarget<RouletteTypeNode> RouletteTypes { get; set; } = RouletteTypeNodeList.Create(null);
    }
}
