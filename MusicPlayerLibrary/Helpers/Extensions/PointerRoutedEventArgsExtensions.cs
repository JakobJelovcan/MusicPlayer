using MusicPlayerLibrary.Constants;
using Windows.Devices.Input;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class PointerRoutedEventArgsExtensions
    {
        public static PointerType GetPointerType(this PointerRoutedEventArgs e, UIElement sender)
        {
            Pointer pointer = e.Pointer;
            if (pointer.PointerDeviceType.Equals(PointerDeviceType.Mouse))
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(sender);
                if (pointerPoint.Properties.IsXButton1Pressed) return PointerType.X1Button;
                if (pointerPoint.Properties.IsXButton2Pressed) return PointerType.X2Button;
                if (pointerPoint.Properties.IsRightButtonPressed) return PointerType.RightButton;
                if (pointerPoint.Properties.IsLeftButtonPressed) return PointerType.LeftButton;
                if (pointerPoint.Properties.IsMiddleButtonPressed) return PointerType.MiddleButton;
            }
            return PointerType.LeftButton;
        }
    }
}
