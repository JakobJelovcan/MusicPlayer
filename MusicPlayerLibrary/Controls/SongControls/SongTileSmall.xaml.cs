using MusicPlayerLibrary.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.SongControls
{
    public sealed partial class SongTileSmall : UserControl
    {
        public SongTileSmall()
        {
            InitializeComponent();
        }

        public SongViewModel SongView
        {
            get => (SongViewModel)GetValue(SongViewProperty);
            set => SetValue(SongViewProperty, value);
        }
        public static readonly DependencyProperty SongViewProperty = DependencyProperty.Register("SongView", typeof(SongViewModel), typeof(SongTileSmall), new PropertyMetadata(null));

        private void SongTile_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void SongTile_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void SongTile_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void SongTile_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void SongTile_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void SongTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is SongViewModel song) SongView = song;
        }
    }
}
