﻿using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.AlbumControls
{
    public sealed partial class AlbumTileTall : UserControl
    {
        public AlbumTileTall()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"AlbumTileTall {GetHashCode()} Constructed");
#endif
        }

        ~AlbumTileTall()
        {
#if DEBUG
            Debug.WriteLine($"AlbumTileTall {GetHashCode()} Finalized");
#endif
        }

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public event GoToArtistEvent GoToArtist
        {
            add => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToArtistEvent> goToArtistEventTable;

        public AlbumModel Album
        {
            get => (AlbumModel)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
        }
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register("Album", typeof(AlbumModel), typeof(AlbumTileTall), new PropertyMetadata(null));

        public bool ImagePointerOver
        {
            get => (bool)GetValue(ImagePointerOverProperty);
            set => SetValue(ImagePointerOverProperty, value);
        }
        public static readonly DependencyProperty ImagePointerOverProperty = DependencyProperty.Register("ImagePointerOver", typeof(bool), typeof(AlbumTileTall), new PropertyMetadata(false));

        private void AlbumTileWide_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is AlbumModel album) Album = album;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Album);
        }

        private void GoToArtistButton_Click(object sender, RoutedEventArgs e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, Album?.ParentArtist);
        }

        private void AlbumImageBorder_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ImagePointerOver = true;
        }

        private void AlbumImageBorder_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ImagePointerOver = false;
        }

        private void AlbumTileTall_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void AlbumTileTall_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void AlbumTileTall_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void AlbumTileTall_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }
    }
}