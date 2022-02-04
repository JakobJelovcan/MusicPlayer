using Microsoft.UI.Xaml.Controls;
using MusicPlayerLibrary.Constants;
using System;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.Info
{
    public class InfoMessage : DependencyObject
    {
        private InfoMessage()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            IsOpen = false;
        }

        static InfoMessage()
        {
            CurrentMessage = new InfoMessage();
        }
        private readonly DispatcherTimer Timer;

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set
            {
                Timer.Stop();
                SetValue(IsOpenProperty, value);
            }
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(InfoMessage), new PropertyMetadata(false));

        public bool IsClosable
        {
            get => (bool)GetValue(IsClosableProperty);
            set => SetValue(IsClosableProperty, value);
        }
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register("IsClosable", typeof(bool), typeof(InfoBar), new PropertyMetadata(true));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InfoMessage), new PropertyMetadata(string.Empty));

        public InfoTileSeverity Severity
        {
            get => (InfoTileSeverity)GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }
        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity", typeof(InfoBarSeverity), typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Informational));

        public static InfoMessage CurrentMessage;

        public static void ShowMessage(string message, InfoTileSeverity severity)
        {
            CurrentMessage ??= new InfoMessage();
            (CurrentMessage.IsOpen, CurrentMessage.Message, CurrentMessage.Severity) = (true, message, severity);
        }

        public static void ShowMessage(string message, InfoTileSeverity severity, bool useTimer)
        {
            CurrentMessage ??= new InfoMessage();
            (CurrentMessage.IsOpen, CurrentMessage.Message, CurrentMessage.Severity) = (true, message, severity);
            if (useTimer)
            {
                CurrentMessage.Timer.Interval = TimeSpan.FromSeconds(5);
                CurrentMessage.Timer.Start();
            }
        }

        public static void ShowMessage(string message, InfoTileSeverity severity, TimeSpan duration)
        {
            CurrentMessage ??= new InfoMessage();
            (CurrentMessage.IsOpen, CurrentMessage.Message, CurrentMessage.Severity) = (true, message, severity);
            CurrentMessage.Timer.Interval = duration;
            CurrentMessage.Timer.Start();
        }

        public static void CloseMessage()
        {
            CurrentMessage.IsOpen = false;
        }

        private void Timer_Tick(object sender, object e)
        {
            IsOpen = false;
            Timer.Stop();
        }
    }
}