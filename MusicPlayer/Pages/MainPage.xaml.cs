using MusicPlayer.Helpers.NavigationHelpers;
using MusicPlayer.Pages;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IMusicPlayerPage
    {
        public MainPage()
        {
            InitializeComponent();
            Window.Current.SetTitleBar(TitleBar);
            Settings.NavigationButtonVisibilityChanged += Settings_NavigationButtonVisibilityChanged;
            Settings.EnableSmallControlBarChanged += Settings_EnableSmallControlBarChanged;
            ForwardButton.Visibility = BackButton.Visibility = Settings.NavigationButtonVisibility ? Visibility.Visible : Visibility.Collapsed;
            ChangeControlTile(Settings.EnableSmallControlBar);
#if DEBUG
            Debug.WriteLine($"MainPage {GetHashCode()} Constructed");
            DebugNavigationItem.Visibility = Visibility.Visible;
#else
            DebugNavigationItem.Visibility = Visibility.Collapsed;
#endif
        }

        ~MainPage()
        {
#if DEBUG
            Debug.WriteLine($"MainPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(MainPage), new PropertyMetadata(null));
        //
        //Navigation
        //
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                if (parameters.PageAction == PageActions.NavigateToPage) ContentFrame.Navigate(parameters.NavigateToPage, parameters.NavigateToPageParameters);
                else if (ContentFrame.Content is null) ContentFrame.Navigate(typeof(HomePage), new PageParameters(MusicPlayer));
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is not null) ContentFrame.Navigate(NavigationHelpers.GetPageTypeFromName((args.InvokedItemContainer as NavigationViewItem).Name), new PageParameters(MusicPlayer));
        }

        private void Settings_NavigationButtonVisibilityChanged(bool newValue)
        {
            ForwardButton.Visibility = BackButton.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Settings_EnableSmallControlBarChanged(bool newValue)
        {
            ChangeControlTile(newValue);
        }

        private void ChangeControlTile(bool value)
        {
            switch (value)
            {
                case true: VisualStateManager.GoToState(this, nameof(SmallControlBarState), true); break;
                case false: VisualStateManager.GoToState(this, nameof(LargeControlBarState), true); break;
            }
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == (ContentFrame?.CurrentSourcePageType))
            {
                if (e.Parameter is PageParameters parameters)
                {
                    if (NavigationHelpers.GetPageContent(ContentFrame?.Content as Page) == parameters.PageParameter)
                    {
                        if (parameters.PageAction == (PageActions.ScrollInToView) && ContentFrame?.Content is IScrollablePage scrollablePage)
                        {
                            (scrollablePage.PageActionTarget, scrollablePage.PageAction) = (parameters.PageActionTarget, parameters.PageAction);
                            scrollablePage.ScrollInToView(parameters.PageActionTarget);
                        }
                        e.Cancel = true;
                    }
                }
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.CanGoBack) BackButton.IsEnabled = true;
            else BackButton.IsEnabled = false;
            if (ContentFrame.CanGoForward) ForwardButton.IsEnabled = true;
            else ForwardButton.IsEnabled = false;
            NavigationView.SelectedItem = NavigationView.MenuItems.FirstOrDefault(I => (I as NavigationViewItem).Name?.Equals(NavigationHelpers.GetPageNameFromType((sender as Frame).Content as Page)) ?? false);
        }

        private void MainPage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            switch (e.GetPointerType(this))
            {
                case PointerType.X1Button:
                    {
                        if (ContentFrame.CanGoBack) ContentFrame.GoBack();
                        e.Handled = true;
                        break;
                    }
                case PointerType.X2Button:
                    {
                        if (ContentFrame.CanGoForward) ContentFrame.GoForward();
                        e.Handled = true;
                        break;
                    }
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(TitleBar);
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.NavigationButtonVisibilityChanged -= Settings_NavigationButtonVisibilityChanged;
            Settings.EnableSmallControlBarChanged -= Settings_EnableSmallControlBarChanged;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoForward) ContentFrame.GoForward();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack) ContentFrame.GoBack();
        }
        //
        //Search
        //
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason.HasFlag(AutoSuggestionBoxTextChangeReason.UserInput) && !args.Reason.HasFlag(AutoSuggestionBoxTextChangeReason.SuggestionChosen) && !args.Reason.HasFlag(AutoSuggestionBoxTextChangeReason.ProgrammaticChange))
            {
                Debug.WriteLine((int)args.Reason);
                List<string> newHints = MusicPlayer.Search.GetHints(sender.Text);
                if (!(sender.ItemsSource is IEnumerable<string> oldHints && !oldHints.Except(newHints).AsEnumerable().Any() && !newHints.Except(oldHints).AsEnumerable().Any())) sender.ItemsSource = newHints;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            MusicPlayer.Search.StartQuery(sender.Text);
            sender.ItemsSource = null;
            ContentFrame.Navigate(typeof(SearchPage), new PageParameters(MusicPlayer));
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = sender.Text.Contains(':') ? $"{sender.Text.Split(':')[0]}:{args.SelectedItem}" : args.SelectedItem.ToString();
        }
        //
        //ControlBar
        //
        private void ControlBar_ChangeView(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FullScreenPage), new PageParameters(MusicPlayer));
        }

        private void ControlBar_PositionChanged(double newValue)
        {
            MusicPlayer.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(newValue);
            MusicPlayer?.LyricsPlayer?.PlayLyrics();
        }

        private async void ControlBar_PlayPreviousClick(object sender, RoutedEventArgs e)
        {
            await MusicPlayer?.PlayPrevious();
        }

        private void ControlBar_PlayPauseClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.PlayPause();
        }

        private async void ControlBar_PlayNextClick(object sender, RoutedEventArgs e)
        {
            await MusicPlayer?.PlayNext();
        }

        private void ControlBar_MuteClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.CycleMute();
        }

        private void ControlBar_ShuffleClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.CycleShuffle();
        }

        private void ControlBar_LoopClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer.CycleLoop();
        }

        private void ControlBar_GoToArtist(object sender, ArtistModel e)
        {
            ContentFrame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void ControlBar_GoToSong(object sender, SongModel e)
        {
            try
            {
                ContentFrame.Navigate(NavigationHelpers.GetPageTypeFromPlayingLocation(MusicPlayer.CurrentPlayingLocation), new PageParameters(MusicPlayer, MusicPlayer?.CurrentPlayingContent, PageActions.ScrollInToView, e));
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
    }
}