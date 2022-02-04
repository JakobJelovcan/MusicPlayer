using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.StorageControls
{
    public sealed partial class StorageTile : UserControl, INotifyPropertyChanged
    {
        public StorageTile()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"StorageTile {GetHashCode()} Constructed");
#endif
        }

        ~StorageTile()
        {
#if DEBUG
            Debug.WriteLine($"StorageTile {GetHashCode()} Finalized");
#endif
        }

        public StorageFolderModel StorageFolder
        {
            get => storageFolder;
            set
            {
                if (storageFolder != value)
                {
                    storageFolder?.UnregisterPropertyChanged(StorageFolder_PropertyChanged);
                    storageFolder = value;
                    storageFolder?.RegisterPropertyChanged(StorageFolder_PropertyChanged);
                    if (value is StorageFolderModel) (FolderName, FolderPath) = (value.Name, value.Path);
                }
            }
        }
        private StorageFolderModel storageFolder;

        public string FolderName
        {
            get => folderName;
            set
            {
                if (folderName != value)
                {
                    folderName = value;
                    RaisePropertyChanged(nameof(FolderName));
                }
            }
        }
        private string folderName;

        public string FolderPath
        {
            get => folderPath;
            set
            {
                if (folderPath != value)
                {
                    folderPath = value;
                    RaisePropertyChanged(nameof(FolderPath));
                }
            }
        }
        private string folderPath;

        public bool ErrorVisibility
        {
            get => errorVisibility;
            set
            {
                if (errorVisibility != value)
                {
                    errorVisibility = value;
                    RaisePropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        private bool errorVisibility;

        public Visibility DeleteButtonVisibility
        {
            get => (Visibility)GetValue(DeleteButtonVisibilityProperty);
            set => SetValue(DeleteButtonVisibilityProperty, value);
        }
        public static readonly DependencyProperty DeleteButtonVisibilityProperty = DependencyProperty.Register("DeleteButtonVisibility", typeof(Visibility), typeof(StorageTile), new PropertyMetadata(Visibility.Visible));
        public event RoutedEventHandler Delete;
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }

        private void StorageFolder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(StorageFolderModel.Exists): ErrorVisibility = !(StorageFolder?.Exists ?? true); break;
            }
        }

        private void StorageTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is StorageFolderModel storageFolder) StorageFolder = storageFolder;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Delete.Invoke(this, e);
        }

        private void StorageTile_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void StorageTile_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void StorageTile_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void StorageTile_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void StorageTile_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
