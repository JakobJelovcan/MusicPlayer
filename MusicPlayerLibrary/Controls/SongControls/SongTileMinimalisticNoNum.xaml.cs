﻿using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.SongControls
{
    public sealed partial class SongTileMinimalisticNoNum : UserControl
    {
        public SongTileMinimalisticNoNum()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"SongTileMinimalisticNoNum {GetHashCode()} Constructed");
#endif
        }

        ~SongTileMinimalisticNoNum()
        {
#if DEBUG
            Debug.WriteLine($"SongTileMinimalisticNoNum {GetHashCode()} Finalized");
#endif
        }

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public SongModel Song
        {
            get => (SongModel)GetValue(SongProperty);
            set
            {
                Song?.UnregisterPropertyChanged(Song_PropertyChanged);
                SetValue(SongProperty, value);
                Song?.RegisterPropertyChanged(Song_PropertyChanged);
                UpdatePlayingStates();
            }
        }
        public static readonly DependencyProperty SongProperty = DependencyProperty.Register("Song", typeof(SongModel), typeof(SongTileMinimalisticNoNum), new PropertyMetadata(null));

        public bool TilePointerOver
        {
            get => (bool)GetValue(TilePointerOverProperty);
            set => SetValue(TilePointerOverProperty, value);
        }
        public static readonly DependencyProperty TilePointerOverProperty = DependencyProperty.Register("TilePointerOver", typeof(bool), typeof(SongTileMinimalisticNoNum), new PropertyMetadata(false));

        private void UpdatePlayingStates()
        {
            switch (Song?.PlayingState)
            {
                case PlayingState.NotPlaying: VisualStateManager.GoToState(this, nameof(NotPlaying), true); break;
                default: VisualStateManager.GoToState(this, nameof(Playing), true); break;
            }
        }

        private void UpdateDisabledState()
        {
            switch (IsEnabled)
            {
                case true: VisualStateManager.GoToState(this, nameof(Normal), true); break;
                default: VisualStateManager.GoToState(this, nameof(Disabled), true); break;
            }
        }

        private void SongTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is SongModel song) Song = song;
        }

        private void Song_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SongModel.PlayingState): UpdatePlayingStates(); break;
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Song);
        }

        private void SongTileMinimalisticNoNum_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Song);
        }

        private void SongTileMinimalisticNoNum_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, nameof(PointerOver), true);
                TilePointerOver = true;
            }
        }

        private void SongTileMinimalisticNoNum_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, nameof(Normal), true);
                TilePointerOver = false;
            }
        }

        private void SongTileMinimalisticNoNum_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled) VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void SongTileMinimalisticNoNum_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled) VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void SongTileMinimalisticNoNum_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateDisabledState();
        }

        private void SongTileMinimalisticNoNum_Loaded(object sender, RoutedEventArgs e)
        {
            Song?.RegisterPropertyChanged(Song_PropertyChanged);
            UpdatePlayingStates();
            UpdateDisabledState();
        }

        private void SongTileMinimalisticNoNum_Unloaded(object sender, RoutedEventArgs e)
        {
            Song?.UnregisterPropertyChanged(Song_PropertyChanged);
        }
    }
}