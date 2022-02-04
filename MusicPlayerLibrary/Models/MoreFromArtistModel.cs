using ExtensionsLibrary.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.Models
{
    public class MoreFromArtistModel : INotifyPropertyChanged
    {
        public ArtistModel Artist
        {
            get => artist;
            set
            {
                if (artist != value)
                {
                    artist = value;
                    RaisePropertyChanged(nameof(Artist));
                }
            }
        }
        private ArtistModel artist;

        public ObservableCollection<AlbumModel> Albums
        {
            get => albums;
            set
            {
                if (albums != value)
                {
                    if (albums != null) albums.CollectionChanged -= Albums_CollectionChanged;
                    albums = value;
                    albums.CollectionChanged += Albums_CollectionChanged;
                    RaisePropertyChanged(nameof(Albums));
                }
            }
        }
        private ObservableCollection<AlbumModel> albums;

        private void Albums_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Visibility));
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MoreFromArtistModel(AlbumModel album)
        {
            Artist = album?.ParentArtist;
            Albums = Artist?.Albums?.Where(A => !A.Equals(album)).OrderByDescending(A => A.Year).ToObservableCollection();
        }
    }
}
