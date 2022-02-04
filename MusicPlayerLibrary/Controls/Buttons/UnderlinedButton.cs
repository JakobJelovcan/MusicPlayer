using ExtensionsLibrary.Extensions;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MusicPlayerLibrary.Controls.Buttons
{
    [TemplatePart(Name = nameof(TextBlock), Type = typeof(TextBlock))]
    [TemplatePart(Name = nameof(Border), Type = typeof(Border))]
    public sealed class UnderlinedButton : Button
    {
        public UnderlinedButton()
        {
            DefaultStyleKey = typeof(UnderlinedButton);
            PointerEntered += UnderlinedButton_PointerEntered;
            PointerExited += UnderlinedButton_PointerExited;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Border = GetTemplateChild(nameof(Border)) as Border;
            TextBlock = GetTemplateChild(nameof(TextBlock)) as TextBlock;
            RegisterPropertyChangedCallback(ForegroundProperty, ForegroundChanged);
        }

        public TextBlock TextBlock { get; private set; }

        public Border Border { get; private set; }

        public Storyboard Animation { get; private set; }

        private void ForegroundChanged(DependencyObject sender, DependencyProperty dp)
        {
            TextBlock.Foreground = Foreground;
            Border.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void UnderlinedButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SetAnimation((Foreground as SolidColorBrush).Color, Colors.Transparent);
            Animation.Completed += Animation_Completed;
            Animation.Begin();
        }

        private void UnderlinedButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetAnimation((Color)Resources["SystemAccentColor"]);
            Animation.Begin();
        }

        private void Animation_Completed(object sender, object e)
        {
            Border.BorderBrush = new SolidColorBrush(Colors.Transparent);
            TextBlock.Foreground = Foreground;
            (sender as Storyboard).Completed -= Animation_Completed;
        }

        private void SetAnimation(Color to)
        {
            SetAnimation(to, to);
        }

        private void SetAnimation(Color tbTo, Color bTo)
        {
            Animation ??= new Storyboard();
            Animation.Stop();
            Animation.Completed -= Animation_Completed;
            Animation.Children.Clear();
            ColorAnimation tbCA = CreateColorAnimation(tbTo);
            Storyboard.SetTarget(tbCA, TextBlock);
            Storyboard.SetTargetProperty(tbCA, "(TextBlock.Foreground).(SolidColorBrush.Color)");
            ColorAnimation bCA = CreateColorAnimation(bTo);
            Storyboard.SetTarget(bCA, Border);
            Storyboard.SetTargetProperty(bCA, "(Border.BorderBrush).(SolidColorBrush.Color)");
            Animation.Children.AddRange(tbCA, bCA);
        }

        private ColorAnimation CreateColorAnimation(Color to)
        {
            return new ColorAnimation { To = to, Duration = TimeSpan.FromMilliseconds(100), EnableDependentAnimation = true };
        }
    }
}
