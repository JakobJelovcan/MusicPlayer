using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Controls.StorageControls;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Info;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.ContentDialogs
{
    public sealed partial class StorageLocationsContentDialog : ContentDialog, INotifyPropertyChanged
    {
        public StorageLocationsContentDialog(MusicPlayerModel musicPlayer)
        {
            InitializeComponent();
            MusicPlayer = musicPlayer;
            StorageFolders = new ObservableCollection<StorageFolderModel>(DBAccess.StorageFolders);
            FoldersToAdd = new List<StorageFolderModel>();
            FoldersToRemove = new List<StorageFolderModel>();
            RaisePropertyChanged(nameof(StorageFolders));
        }

        public MusicPlayerModel MusicPlayer { get; private set; }

        public ObservableCollection<StorageFolderModel> StorageFolders { get; private set; }

        public List<StorageFolderModel> FoldersToAdd { get; private set; }

        public List<StorageFolderModel> FoldersToRemove { get; private set; }

        public Visibility RestartMessageVisibility
        {
            get => (Visibility)GetValue(RestartMessageVisibilityProperty);
            set => SetValue(RestartMessageVisibilityProperty, value);
        }
        public static readonly DependencyProperty RestartMessageVisibilityProperty = DependencyProperty.Register("RestartMessageVisibility", typeof(Visibility), typeof(StorageLocationsContentDialog), new PropertyMetadata(Visibility.Collapsed));

        private void AddAFolder(StorageFolderModel storageFolder)
        {
            if (FoldersToRemove.FirstOrDefault(F => F.Path == storageFolder.Path) is StorageFolderModel storageFolderModel) FoldersToRemove.Remove(storageFolderModel);
            else if (!StorageFolders.Any(F => F.Path == storageFolder.Path))
            {
                FoldersToAdd.AddIfDoesntContain(storageFolder, F => F.Path);
                StorageFolders.AddIfDoesntContain(storageFolder, F => F.Path);
            }
            RestartMessageVisibility = (FoldersToAdd.Any() || FoldersToRemove.Any()) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RemoveAFolder(StorageFolderModel storageFolder)
        {
            if (FoldersToAdd.FirstOrDefault(F => F.Path == storageFolder.Path) is StorageFolderModel storageFolderModel) FoldersToAdd.Remove(storageFolderModel);
            else FoldersToRemove.AddIfDoesntContain(storageFolder, P => P.Path);
            StorageFolders.Remove(StorageFolders.FirstOrDefault(F => F.Path == storageFolder.Path));
            RestartMessageVisibility = (FoldersToAdd.Any() || FoldersToRemove.Any()) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void CloseAndReload_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int oldCount = MusicPlayer.Songs.Count;
            FoldersToRemove.ForEach(F => DBAccess.StorageFolders.Remove(F));
            FoldersToAdd.ForEach(F => DBAccess.StorageFolders.AddOrUpdate(F));
            await DBAccess.SaveChangesAsync();
            await MusicPlayer.LoadDataFromStorageAsync();
            int newCount = MusicPlayer.Songs.Count;
            if (newCount - oldCount >= 0) InfoMessage.ShowMessage($"{newCount - oldCount} new songs have been added.", InfoTileSeverity.Success, true);
            Hide();
        }

        private void Close_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            FoldersToRemove.ForEach(F => DBAccess.StorageFolders.Remove(F));
            FoldersToAdd.ForEach(F => DBAccess.StorageFolders.AddOrUpdate(F));
            DBAccess.SaveChanges();
            Hide();
        }

        private async void AddANewLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (await StorageFolderHelpers.GetStorageFolderModelAsync() is StorageFolderModel storageFolder) AddAFolder(storageFolder);
        }

        private void StorageTile_Delete(object sender, RoutedEventArgs e)
        {
            RemoveAFolder((sender as StorageTile).StorageFolder);
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}