using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Lyrics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.LyricsControls
{
    public sealed partial class LyricTile : UserControl
    {
        public LyricTile()
        {
            InitializeComponent();
        }

        public LyricModel Lyric
        {
            get => lyric;
            set
            {
                if (lyric != value)
                {
                    lyric?.UnregisterPropertyChanged(Lyric_PropertyChanged);
                    lyric = value;
                    SetState();
                    (MicHeadColor, MicHandleColor) = GetColors(lyric.Singer);
                    lyric?.RegisterPropertyChanged(Lyric_PropertyChanged);
                }
            }
        }
        private LyricModel lyric;

        public SolidColorBrush MicHeadColor
        {
            get => (SolidColorBrush)GetValue(MicHeadColorProperty);
            set => SetValue(MicHeadColorProperty, value);
        }
        public static readonly DependencyProperty MicHeadColorProperty = DependencyProperty.Register("MicHeadColor", typeof(SolidColorBrush), typeof(LyricTile), new PropertyMetadata(new SolidColorBrush()));

        public SolidColorBrush MicHandleColor
        {
            get => (SolidColorBrush)GetValue(MicHandleColorProperty);
            set => SetValue(MicHandleColorProperty, value);
        }
        public static readonly DependencyProperty MicHandleColorProperty = DependencyProperty.Register("MicHandleColor", typeof(SolidColorBrush), typeof(LyricTile), new PropertyMetadata(new SolidColorBrush()));

        private void LyricTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is LyricModel lyric) Lyric = lyric;
        }

        private void Lyric_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetState();
        }

        private void SetState()
        {
            switch (Lyric?.IsHighlighted ?? false)
            {
                case true: VisualStateManager.GoToState(this, nameof(Highlighted), true); break;
                case false: VisualStateManager.GoToState(this, nameof(Normal), true); break;
            }
        }

        private (SolidColorBrush HeadColor, SolidColorBrush HandleColor) GetColors(LyricsSinger singer)
        {
            switch (singer)
            {
                case LyricsSinger.Male: return (new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue), new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue));
                case LyricsSinger.Female: return (new SolidColorBrush(Windows.UI.Colors.LightPink), new SolidColorBrush(Windows.UI.Colors.LightPink));
                case LyricsSinger.Duet: return (new SolidColorBrush(Windows.UI.Colors.LightPink), new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue));
                default: return (new SolidColorBrush(Windows.UI.Colors.Transparent), new SolidColorBrush(Windows.UI.Colors.Transparent));
            }
        }

        private void LyricTile_Unloaded(object sender, RoutedEventArgs e)
        {
            Lyric?.UnregisterPropertyChanged(Lyric_PropertyChanged);
        }
    }
}