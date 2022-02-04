using Windows.Foundation;
using Windows.Media.Playback;

namespace ExtensionsLibrary.Extensions
{
    public static class PositionChangedExtensions
    {
        public static void RegisterPositionChanged(this MediaPlaybackSession session, TypedEventHandler<MediaPlaybackSession, object> handler)
        {
            session.PositionChanged += handler;
        }

        public static void UnregisterPositionChanged(this MediaPlaybackSession session, TypedEventHandler<MediaPlaybackSession, object> handler)
        {
            session.PositionChanged -= handler;
        }
    }
}
