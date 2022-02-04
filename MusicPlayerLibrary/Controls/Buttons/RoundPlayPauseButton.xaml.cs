using MusicPlayerLibrary.Constants;
using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Buttons
{
    public sealed partial class RoundPlayPauseButton : UserControl
    {
        public RoundPlayPauseButton()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"RoundPlayPauseButton {GetHashCode()} Constructed");
#endif
        }

        ~RoundPlayPauseButton()
        {
#if DEBUG
            Debug.WriteLine($"RoundPlayPauseButton {GetHashCode()} Finalized");
#endif
        }

        public float ButtonSize
        {
            get => (float)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }
        public static readonly DependencyProperty ButtonSizeProperty = DependencyProperty.Register("ButtonSize", typeof(float), typeof(RoundPlayPauseButton), new PropertyMetadata(50));

        public float EllipseSize
        {
            get => (float)GetValue(EllipseSizeProperty);
            set => SetValue(EllipseSizeProperty, value);
        }
        public static readonly DependencyProperty EllipseSizeProperty = DependencyProperty.Register("EllipseSize", typeof(float), typeof(RoundPlayPauseButton), new PropertyMetadata(32));

        public float EllipsePointerOverSize
        {
            get => (float)GetValue(EllipsePointerOverSizeProperty);
            set => SetValue(EllipsePointerOverSizeProperty, value);
        }
        public static readonly DependencyProperty EllipsePointerOverSizeProperty = DependencyProperty.Register("EllipsePointerOverSize", typeof(float), typeof(RoundPlayPauseButton), new PropertyMetadata(34));

        public TimeSpan AnimationDuration
        {
            get => (TimeSpan)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(RoundPlayPauseButton), new PropertyMetadata(TimeSpan.Zero));

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(RoundPlayPauseButton), new PropertyMetadata(default));

        public float IconFontSize
        {
            get => (float)GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }
        public static readonly DependencyProperty IconFontSizeProperty = DependencyProperty.Register("IconFontSize", typeof(float), typeof(RoundPlayPauseButton), new PropertyMetadata(12));

        public string ButtonSymbol
        {
            get => (string)GetValue(ButtonSymbolProperty);
            set => SetValue(ButtonSymbolProperty, value);
        }
        public static readonly DependencyProperty ButtonSymbolProperty = DependencyProperty.Register("ButtonSymbol", typeof(string), typeof(RoundPlayPauseButton), new PropertyMetadata(string.Empty));

        public PlayingState PlayingState
        {
            get => (PlayingState)GetValue(PlayingStateProperty);
            set
            {
                SetValue(PlayingStateProperty, value);
                ButtonSymbol = value == PlayingState.Playing ? Symbols.Pause : Symbols.PlaySolid;
                UpdateButtonOpacityStates();
            }
        }
        public static readonly DependencyProperty PlayingStateProperty = DependencyProperty.Register("PlayingState", typeof(PlayingState), typeof(RoundPlayPauseButton), new PropertyMetadata(PlayingState.NotPlaying));

        public bool ButtonPointerOver
        {
            get => (bool)GetValue(ButtonPointerOverProperty);
            set
            {
                SetValue(ButtonPointerOverProperty, value);
                UpdateButtonOpacityStates();
            }
        }
        public static readonly DependencyProperty ButtonPointerOverProperty = DependencyProperty.Register("ButtonPointerOver", typeof(bool), typeof(RoundPlayPauseButton), new PropertyMetadata(false));

        public PlayButtonType ButtonType
        {
            get => (PlayButtonType)GetValue(ButtonTypeProperty);
            set
            {
                SetValue(ButtonTypeProperty, value);
                SetValues();
            }
        }
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register("ButtonType", typeof(PlayButtonType), typeof(RoundPlayPauseButton), new PropertyMetadata(PlayButtonType.None));

        public string Track
        {
            get => (string)GetValue(TrackProperty);
            set => SetValue(TrackProperty, value);
        }
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track", typeof(string), typeof(RoundPlayPauseButton), new PropertyMetadata(string.Empty));

        public Visibility TrackVisibility
        {
            get => (Visibility)GetValue(TrackVisibilityProperty);
            set => SetValue(TrackVisibilityProperty, value);
        }
        public static readonly DependencyProperty TrackVisibilityProperty = DependencyProperty.Register("TrackVisibility", typeof(Visibility), typeof(RoundPlayPauseButton), new PropertyMetadata(Visibility.Collapsed));

        private Microsoft.Toolkit.Uwp.UI.Media.AcrylicBrush BackgroundAcrylic => new Microsoft.Toolkit.Uwp.UI.Media.AcrylicBrush { TintColor = Color.FromArgb(0xFF, 0x00, 0x00, 0x00), BackgroundSource = AcrylicBackgroundSource.Backdrop, BlurAmount = 1f, TintOpacity = .6f, Opacity = 1 };
        public event RoutedEventHandler PlayButtonClick;

        private void UpdateButtonOpacityStates()
        {
            switch (PlayingState != PlayingState.NotPlaying || ButtonPointerOver)
            {
                case true: if(ButtonOpacityStates.CurrentState != Visible) VisualStateManager.GoToState(this, nameof(Visible), true); break;
                case false: if(ButtonOpacityStates.CurrentState != Hidden) VisualStateManager.GoToState(this, nameof(Hidden), true); break;
            }
        }

        private void SetValues()
        {
            switch (ButtonType)
            {
                case PlayButtonType.Small: (ButtonSize, EllipseSize, EllipsePointerOverSize, AnimationDuration, BackgroundBrush, IconFontSize) = (50f, 32f, 34f, TimeSpan.Zero, BackgroundAcrylic, 12f); VisualStateManager.GoToState(this, nameof(Static), true); break;
                case PlayButtonType.SmallNoTint: (ButtonSize, EllipseSize, EllipsePointerOverSize, AnimationDuration, BackgroundBrush, IconFontSize) = (50f, 32f, 34f, TimeSpan.Zero, new SolidColorBrush(Colors.Transparent), 12f); VisualStateManager.GoToState(this, nameof(Dynamic), true); break;
                case PlayButtonType.Medium: (ButtonSize, EllipseSize, EllipsePointerOverSize, AnimationDuration, BackgroundBrush, IconFontSize) = (60f, 36f, 38f, TimeSpan.Zero, BackgroundAcrylic, 14f); VisualStateManager.GoToState(this, nameof(Static), true); break;
                case PlayButtonType.Large: (ButtonSize, EllipseSize, EllipsePointerOverSize, AnimationDuration, BackgroundBrush, IconFontSize) = (70f, 60f, 64f, TimeSpan.FromMilliseconds(90), BackgroundAcrylic, 30f); VisualStateManager.GoToState(this, nameof(Static), true); break;
            }
        }

        private void PlayButton_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void PlayButton_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButtonClick.Invoke(this, e);
        }

        private void RoundPlayPauseButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsEnabled) VisualStateManager.GoToState(this, nameof(Hidden), true);
        }
    }
}