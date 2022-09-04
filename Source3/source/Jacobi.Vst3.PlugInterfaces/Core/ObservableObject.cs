using System.ComponentModel;

namespace Steinberg.Vst3
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
