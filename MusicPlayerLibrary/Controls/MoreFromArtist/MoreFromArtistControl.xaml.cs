using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.MoreFromArtist
{
    public sealed partial class MoreFromArtistControl : UserControl
    {
        public MoreFromArtistControl()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"MoreFromArtist {GetHashCode()} Constructed");
#endif
        }

        ~MoreFromArtistControl()
        {
#if DEBUG
            Debug.WriteLine($"MoreFromArtist {GetHashCode()} Constructed");
#endif
        }

        public MoreFromArtistModel MoreFromArtist
        {
            get => (MoreFromArtistModel)GetValue(MoreFromArtistProperty);
            set
            {
                SetValue(MoreFromArtistProperty, value);
                MoreFromArtistVisibility = (MoreFromArtist?.Albums?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public static readonly DependencyProperty MoreFromArtistProperty = DependencyProperty.Register("MoreFromArtist", typeof(MoreFromArtistModel), typeof(MoreFromArtistControl), new PropertyMetadata(null));

        public Visibility MoreFromArtistVisibility
        {
            get => (Visibility)GetValue(MoreFromArtistVisibilityProperty);
            set => SetValue(MoreFromArtistVisibilityProperty, value);
        }
        public static readonly DependencyProperty MoreFromArtistVisibilityProperty = DependencyProperty.Register("MoreFromArtistVisibility", typeof(Visibility), typeof(MoreFromArtistModel), new PropertyMetadata(Visibility.Visible));

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public event GoToAlbumEvent GoToAlbum
        {
            add => EventRegistrationTokenTable<GoToAlbumEvent>.GetOrCreateEventRegistrationTokenTable(ref goToAlbumEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToAlbumEvent>.GetOrCreateEventRegistrationTokenTable(ref goToAlbumEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToAlbumEvent> goToAlbumEventTable;

        public event GoToArtistEvent GoToArtist
        {
            add => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToArtistEvent> goToArtistEventTable;

        private void MoreFromArtistControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MoreFromArtistModel moreFromArtist) MoreFromArtist = moreFromArtist;
        }

        private void MoreFromArtistListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            goToAlbumEventTable?.InvocationList?.Invoke(this, e.ClickedItem as AlbumModel);
        }

        private void GoToArtistButton_Click(object sender, RoutedEventArgs e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, MoreFromArtist?.Artist);
        }

        private void AlbumTileTall_PlayPause(object sender, BaseMusicModel e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, e);
        }

        private void AlbumTileTall_GoToArtist(object sender, ArtistModel e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, e);
        }
    }
}