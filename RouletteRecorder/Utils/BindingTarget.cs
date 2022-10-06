using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RouletteRecorder.Utils
{
    public class BindingTarget : INotifyPropertyChanged
    {
        protected void EmitPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StaticBindingTarget<T> : BindingTarget where T : new()
    {
        public static T Instance { get; } = new T();
    }

    public class ListBindingTarget<T> : ObservableCollection<T>
    {
        public ListBindingTarget() : base() { }
        public ListBindingTarget(List<T> list) : base(list) { }
        public ListBindingTarget(IEnumerable<T> collection) : base(collection) { }

        public void EmitCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        public void EmitPropertyChanged(PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
    }
}
