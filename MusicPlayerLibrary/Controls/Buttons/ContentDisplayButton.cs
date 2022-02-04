using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Events;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MusicPlayerLibrary.Controls.Buttons
{
    public sealed class ContentDisplayButton : Button
    {
        public ContentDisplayButton()
        {
            DefaultStyleKey = typeof(ContentDisplayButton);
            Click += This_Click;
        }

        private void This_Click(object sender, RoutedEventArgs e)
        {
            ContentDisplay = (ContentDisplay == ContentDisplay.Less) ? ContentDisplay.More : ContentDisplay.Less;
        }

        public string Symbol
        {
            get => (string)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(string), typeof(ContentDisplayButton), new PropertyMetadata(Symbols.ChevronDown));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ContentDisplayButton), new PropertyMetadata(ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/ShowButtonResources").GetString("Show_More/Text")));

        public ContentDisplay ContentDisplay
        {
            get => contentDisplay;
            set
            {
                if (contentDisplay != value)
                {
                    contentDisplay = value;
                    SetContent();
                    displayChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }
        private ContentDisplay contentDisplay;

        private void SetContent()
        {
            switch (ContentDisplay)
            {
                case ContentDisplay.Less: (Symbol, Text) = (Symbols.ChevronDown, ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/ShowButtonResources").GetString("Show_More/Text")); break;
                case ContentDisplay.More: (Symbol, Text) = (Symbols.ChevronUp, ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/ShowButtonResources").GetString("Show_Less/Text")); break;
            }
        }

        public event ContentDisplayChanged ContentDisplayChanged
        {
            add => EventRegistrationTokenTable<ContentDisplayChanged>.GetOrCreateEventRegistrationTokenTable(ref displayChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<ContentDisplayChanged>.GetOrCreateEventRegistrationTokenTable(ref displayChangedEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<ContentDisplayChanged> displayChangedEventTable;
    }
}