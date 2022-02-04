using Windows.Storage;

namespace ExtensionsLibrary.Extensions
{
    public static class ApplicationDataContainerExtensions
    {
        public static object TryGetSetting(this ApplicationDataContainer container, string key, object defaultValue = default)
        {
            try
            {
                return container.Values[key] ?? defaultValue;
            }
            catch
            {
                container.Values.Add(key, defaultValue);
                return defaultValue;
            }
        }

        public static void TrySaveSetting(this ApplicationDataContainer container, string key, object value)
        {
            try
            {
                container.Values[key] = value;
            }
            catch { }
        }
    }
}
