using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MusicPlayerLibrary.Controls.HorizontalListView
{
    [TemplatePart(Name = nameof(BackButton), Type = typeof(Button))]
    [TemplatePart(Name = nameof(ForwardButton), Type = typeof(Button))]
    [TemplatePart(Name = nameof(ScrollViewer), Type = typeof(ScrollViewer))]
    [TemplatePart(Name = nameof(ItemsPresenter), Type = typeof(ItemsPresenter))]
    public sealed class HorizontalListView : ListView
    {
        public HorizontalListView()
        {
            DefaultStyleKey = typeof(HorizontalListView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BackButton = GetTemplateChild(nameof(BackButton)) as Button;
            ForwardButton = GetTemplateChild(nameof(ForwardButton)) as Button;
            ScrollViewer = GetTemplateChild(nameof(ScrollViewer)) as ScrollViewer;
            ItemsPresenter = GetTemplateChild(nameof(ItemsPresenter)) as ItemsPresenter;
            ScrollViewer.SizeChanged += ScrollViewer_SizeChanged;
            BackButton.Click += BackButton_Click;
            ForwardButton.Click += ForwardButton_Click;
            ItemsPresenter.SizeChanged += ItemsPresenter_SizeChanged;
            ScrollViewer.Loaded += ScrollViewer_Loaded;
            ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            ScrollViewer.PointerWheelChanged += ScrollViewer_PointerWheelChanged;
        }

        public Button BackButton { get; private set; }

        public Button ForwardButton { get; private set; }

        public ItemsPresenter ItemsPresenter { get; private set; }

        public ScrollViewer ScrollViewer { get; private set; }

        private void SetIsEnabledOnScrollViewerButtons()
        {
            BackButton.IsEnabled = !(ScrollViewer.HorizontalOffset == 0);
            ForwardButton.IsEnabled = !(ScrollViewer.HorizontalOffset == ScrollViewer.ScrollableWidth);
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ChangeView(ScrollViewer.HorizontalOffset + 420, null, null, false);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ChangeView(ScrollViewer.HorizontalOffset - 420, null, null, false);
        }

        private void ItemsPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetIsEnabledOnScrollViewerButtons();
        }

        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPointProperties properties = e.GetCurrentPoint(sender as UIElement).Properties;
            if (properties.IsHorizontalMouseWheel)
            {
                e.Handled = true;
                (sender as ScrollViewer).ChangeView((sender as ScrollViewer).HorizontalOffset + properties.MouseWheelDelta, null, null, false);
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            SetIsEnabledOnScrollViewerButtons();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            SetIsEnabledOnScrollViewerButtons();
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetIsEnabledOnScrollViewerButtons();
        }
    }
}