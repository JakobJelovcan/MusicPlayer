using MusicPlayerLibrary.Constants;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Buttons
{
    public sealed partial class PlayPauseButton : UserControl, INotifyPropertyChanged
    {
        public PlayPauseButton()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"PlayPauseButton {GetHashCode()} Constructed");
#endif
        }

        ~PlayPauseButton()
        {
#if DEBUG
            Debug.WriteLine($"PlayPauseButton {GetHashCode()} Finalized");
#endif
        }

        private string ButtonSymbol => CurrentPlayingState.Equals(PlayingState.Playing) ? Symbols.Pause : Symbols.PlaySolid;

        private string ButtonText => CurrentPlayingState.Equals(PlayingState.Playing) ? ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/PlayPauseButtonResources").GetString("Pause/Text") : ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/PlayPauseButtonResources").GetString("Play/Text");

        public event RoutedEventHandler Click;

        public PlayingState CurrentPlayingState
        {
            get => currentPlayingState;
            set
            {
                if (currentPlayingState != value)
                {
                    currentPlayingState = value;
                    RaisePropertyChanged(nameof(ButtonSymbol), nameof(ButtonText));
                }
            }
        }
        private PlayingState currentPlayingState;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click.Invoke(sender, e);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
