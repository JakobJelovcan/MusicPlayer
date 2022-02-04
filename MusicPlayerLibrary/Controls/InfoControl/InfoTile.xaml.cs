using MusicPlayerLibrary.Constants;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.InfoControl
{
    public sealed partial class InfoTile : UserControl
    {
        public InfoTile()
        {
            InitializeComponent();
            HideTimer = new DispatcherTimer { Interval = TranslationTransition.Duration };
            HideTimer.Tick += HideTimer_Tick;
        }
        private readonly DispatcherTimer HideTimer;

        public InfoTileSeverity Severity
        {
            get => (InfoTileSeverity)GetValue(SeverityProperty);
            set
            {
                SetValue(SeverityProperty, value);
                UpdateInfoVisualState();
            }
        }
        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity", typeof(InfoTileSeverity), typeof(InfoTile), new PropertyMetadata(0));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InfoTile), new PropertyMetadata(string.Empty));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set
            {
                if (value != IsOpen)
                {
                    switch (value)
                    {
                        case true: Open(); break;
                        case false: Close(); break;
                    }
                    UpdateInfoVisualState();
                }
                SetValue(IsOpenProperty, value);
            }
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(InfoTile), new PropertyMetadata(false));

        private void Open()
        {
            Visibility = Visibility.Visible;
            Translation = new Vector3(0, 0, 0);
        }

        private void Close()
        {
            Translation = new Vector3(0, -32, 0);
            HideTimer.Start();
        }

        private void UpdateInfoVisualState()
        {
            switch (Severity)
            {
                case InfoTileSeverity.Informational: VisualStateManager.GoToState(this, nameof(Informational), true); break;
                case InfoTileSeverity.Success: VisualStateManager.GoToState(this, nameof(Success), true); break;
                case InfoTileSeverity.Warning: VisualStateManager.GoToState(this, nameof(Warning), true); break;
                case InfoTileSeverity.Error: VisualStateManager.GoToState(this, nameof(Error), true); break;
                default: throw new ArgumentException();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            IsOpen = false;
        }

        private void HideTimer_Tick(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
            HideTimer.Stop();
        }
    }
}