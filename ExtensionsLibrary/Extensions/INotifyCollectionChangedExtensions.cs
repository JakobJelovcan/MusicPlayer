using System.Collections.Specialized;

namespace ExtensionsLibrary.Extensions
{
    public static class INotifyCollectionChangedExtensions
    {
        public static void RegisterPropertyChanged(this INotifyCollectionChanged notifyCollectionChanged, NotifyCollectionChangedEventHandler handler)
        {
            notifyCollectionChanged.CollectionChanged += handler;
        }

        public static void UnregisterPropertyChanged(this INotifyCollectionChanged notifyCollectionChanged, NotifyCollectionChangedEventHandler handler)
        {
            notifyCollectionChanged.CollectionChanged -= handler;
        }
    }
}