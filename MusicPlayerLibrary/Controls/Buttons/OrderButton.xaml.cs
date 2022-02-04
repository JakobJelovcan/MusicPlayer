using MusicPlayerLibrary.Constants;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Buttons
{
    public sealed partial class OrderButton : UserControl
    {
        public bool IsActive
        {
            set => UpdateIsActive(value);
        }

        public OrderButton()
        {
            InitializeComponent();
        }

        public Order ItemOrder
        {
            get => itemOrder;
            set
            {
                if (itemOrder != value)
                {
                    itemOrder = value;
                    Symbol = (ItemOrder == Order.Ascending) ? Symbols.ChevronUp : Symbols.ChevronDown;
                }
            }
        }
        private Order itemOrder;

        public string Symbol
        {
            get => (string)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(string), typeof(OrderButton), new PropertyMetadata(Symbols.ChevronUp));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(OrderButton), new PropertyMetadata(string.Empty));
        public event RoutedEventHandler Click;

        private void UpdateIsActive(bool value)
        {
            if (value) VisualStateManager.GoToState(this, nameof(Active), true);
            else VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click.Invoke(this, e);
        }
    }
}