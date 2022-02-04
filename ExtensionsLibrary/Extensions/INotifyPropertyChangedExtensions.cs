using System.ComponentModel;

namespace ExtensionsLibrary.Extensions
{
    public static class INotifyPropertyChangedExtensions
    {
        public static void RegisterPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, PropertyChangedEventHandler handler)
        {
            notifyPropertyChanged.PropertyChanged += handler;
        }

        public static void UnregisterPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, PropertyChangedEventHandler handler)
        {
            notifyPropertyChanged.PropertyChanged -= handler;
        }
    }
}
