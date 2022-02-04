using MusicMetaDataLibrary.ID3v2;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DebugPage : Page, IMusicPlayerPage
    {
        public DebugPage()
        {
            InitializeComponent();
        }

        ~DebugPage()
        {

        }

        public MusicPlayerModel MusicPlayer { get; private set; }

        public StorageFile SongFile { get; private set; }

        public new ID3Tag Tag { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters pageParameters) MusicPlayer = pageParameters.MusicPlayer;
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Album
        {
            get => (string)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
        }
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register("Album", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Artist
        {
            get => (string)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public static readonly DependencyProperty ArtistProperty = DependencyProperty.Register("Artist", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Year
        {
            get => (string)GetValue(YearProperty);
            set => SetValue(YearProperty, value);
        }
        public static readonly DependencyProperty YearProperty = DependencyProperty.Register("Year", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Track
        {
            get => (string)GetValue(TrackProperty);
            set => SetValue(TrackProperty, value);
        }
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public BitmapImage Image
        {
            get => (BitmapImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(DebugPage), new PropertyMetadata(null));

        public string Genre
        {
            get => (string)GetValue(GenreProperty);
            set => SetValue(GenreProperty, value);
        }
        public static readonly DependencyProperty GenreProperty = DependencyProperty.Register("Genre", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Composers
        {
            get => (string)GetValue(ComposersProperty);
            set => SetValue(ComposersProperty, value);
        }
        public static readonly DependencyProperty ComposersProperty = DependencyProperty.Register("Composers", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        public string Writers
        {
            get => (string)GetValue(WritersProperty);
            set => SetValue(WritersProperty, value);
        }
        public static readonly DependencyProperty WritersProperty = DependencyProperty.Register("Writers", typeof(string), typeof(DebugPage), new PropertyMetadata(string.Empty));

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".mp3");
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            if (await fileOpenPicker.PickSingleFileAsync() is StorageFile storageFile)
            {
                SongFile = storageFile;
                Tag = new ID3Tag();
                await Tag.LoadTagAsync(storageFile);
                Tag.Clear();
                //Title = Tag.Title;
                //Album = Tag.Album;
                //Artist = Tag.Artist;
                //Track = Tag.Track.ToString();
                //Year = Tag.Year.ToString();
                //Image = await Tag.Picture.GetBitmapAsync();
                //Genre = Tag.Genre;
                //Writers = string.Join("; ", Tag.Writers);
                //Composers = string.Join("; ", Tag.Composers);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Tag.Title = Title;
            Tag.Album = Album;
            Tag.Artist = Artist;
            Tag.Year = int.Parse(Year);
            Tag.Track = uint.Parse(Track);
            Tag.Genre = Genre;
            Tag.Writers = Writers.Replace("; ", ";").Split(";");
            Tag.Composers = Composers.Replace("; ", ";").Split(";");
            await Tag.SaveTagAsync();
        }

        private async void SetImageButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            if (await fileOpenPicker.PickSingleFileAsync() is StorageFile storageFile)
            {
                await Tag.Picture.SetImageAsync(storageFile, BitmapEncoder.JpegEncoderId);
                await Tag.SaveTagAsync();
            }
        }

        private async void LoadFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            int i = 0;
            if (await folderPicker.PickSingleFolderAsync() is StorageFolder storageFolder)
            {
                System.Collections.Generic.IEnumerable<StorageFile> files = await StorageFolderHelpers.GetSongFilesFromFolderAsync(storageFolder);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ID3Tag tag;
                foreach (StorageFile file in files)
                {
                    try
                    {
                        tag = new ID3Tag();
                        await tag.LoadTagAsync(file);
                        ++i;
                        tag.Clear();
                    }
                    catch (Exception)
                    {
                    }
                }
                stopwatch.Stop();
                DurationTextBlock.Text = $"{i} files were loaded in {stopwatch.ElapsedMilliseconds} ms or {stopwatch.ElapsedTicks} ticks";
            }
        }

        private async void LoadFolder1Button_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            int i = 0;
            if (await folderPicker.PickSingleFolderAsync() is StorageFolder storageFolder)
            {
                System.Collections.Generic.IEnumerable<StorageFile> files = await StorageFolderHelpers.GetSongFilesFromFolderAsync(storageFolder);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (StorageFile file in files)
                {
                    try
                    {
                        MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                        ++i;
                    }
                    catch (Exception)
                    {
                    }
                }
                stopwatch.Stop();
                DurationTextBlock.Text = $"{i} files were loaded in {stopwatch.ElapsedMilliseconds} ms or {stopwatch.ElapsedTicks} ticks";
            }
        }
    }
}