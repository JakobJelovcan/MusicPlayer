using Microsoft.Toolkit.Uwp.Notifications;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;

namespace MusicPlayerLibrary.Helpers.LiveTilesHelpers
{
    public static class LiveTilesHelpers
    {
        private static bool TileCleared;

        public static void LoadLiveTiles(LiveTileStyle liveTilesStyle, MusicPlayerModel musicPlayer)
        {
            TileBindingContentPhotos content = new TileBindingContentPhotos();
            foreach (string path in GetLiveTilesPhotos(liveTilesStyle, musicPlayer)) content.Images.Add(new TileBasicImage() { Source = path });
            TileContent tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,
                    TileMedium = new TileBinding() { Content = content, },
                    TileWide = new TileBinding() { Content = content, },
                    TileLarge = new TileBinding() { Content = content, }
                }
            };
            // Create the tile notification
            TileNotification tileNotif = new TileNotification(tileContent.GetXml());
            // And send the notification to the primary tile
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            updater.Update(tileNotif);
        }

        public static void LoadCurrentSongLiveTile(SongModel currentSong)
        {
            TileBindingContentAdaptive content = new TileBindingContentAdaptive()
            {
                TextStacking = TileTextStacking.Bottom,
                Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 1,
                                    Children =
                                    {
                                        new AdaptiveText()
                                        {
                                            Text = currentSong.Title,
                                            HintStyle = AdaptiveTextStyle.Base
                                        },
                                        new AdaptiveText()
                                        {
                                            Text = currentSong.Artist,
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        }
                                    }
                                }
                            }
                        }
                    },
                BackgroundImage = new TileBackgroundImage()
                {
                    Source = currentSong?.Image.RelativePath,
                    HintOverlay = 60
                }
            };
            TileContent tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.None,
                    TileMedium = new TileBinding() { Content = content },
                    TileWide = new TileBinding() { Content = content },
                    TileLarge = new TileBinding() { Content = content }
                }
            };
            // Create the tile notification
            TileNotification tileNotif = new TileNotification(tileContent.GetXml());
            // And send the notification to the primary tile
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            if (!TileCleared)
            {
                TileCleared = true;
                updater.Clear();
            }
            updater.Update(tileNotif);
        }

        private static IEnumerable<string> GetLiveTilesPhotos(LiveTileStyle liveTilesStyle, MusicPlayerModel musicPlayer)
        {
            switch (liveTilesStyle)
            {
                case LiveTileStyle.AlbumsForYou: return musicPlayer.AlbumsForYou.Take(10).Select(A => A?.Image.RelativePath);
                case LiveTileStyle.MostPlayed: return musicPlayer.MostPlayed.Take(10).Select(I => I?.Image.RelativePath);
                default: return musicPlayer.LastPlayed.Take(10).Select(I => I?.Image.RelativePath);
            }
        }
    }
}
