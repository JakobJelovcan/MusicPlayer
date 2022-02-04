using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Helpers.LiveTilesHelpers;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            RequestedTheme = Settings.ApplicationTheme == ElementTheme.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }

        private MusicPlayerModel MusicPlayer;

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Window.Current.Content is not Frame rootFrame)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }
            if (!e.PrelaunchActivated)
            {
                if (rootFrame.Content == null) await Launch(rootFrame, e);
                Window.Current.Activate();
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            LiveTilesHelpers.LoadLiveTiles(LiveTileStyle.LastPlayed, MusicPlayer);
            DBAccess.SaveChanges();
            Settings.SaveChanges();
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected override async void OnFileActivated(FileActivatedEventArgs args)
        {
            base.OnFileActivated(args);
            if (Window.Current.Content is not Frame rootFrame)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null) await Launch(rootFrame, args); ;
            Window.Current.Activate();
        }

        private async Task Launch(Frame rootFrame, IActivatedEventArgs args)
        {
            if (MusicPlayer is null)
            {
                MusicPlayer ??= new MusicPlayerModel();
                await StorageConstants.LoadFolders();
                MusicPlayer.InitializeAsync(args).FireAndForget();
            }
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            rootFrame.Navigate(typeof(MainPage), new PageParameters(MusicPlayer));
        }
    }
}