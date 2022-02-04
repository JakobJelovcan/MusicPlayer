using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicPlayerLibrary.Models
{
    public class StorageFolderModel : IEntity, INotifyPropertyChanged
    {
        public StorageFolderModel()
        {

        }

        public StorageFolderModel(StorageFolder storageFolder)
        {
            Name = storageFolder.DisplayName;
            Path = storageFolder.Path;
            Exists = true;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool Exists
        {
            get => exists;
            set
            {
                if (exists != value)
                {
                    exists = value;
                    RaisePropertyChanged(nameof(Exists));
                }
            }
        }
        private bool exists;

        public async Task<StorageFolder> TryGetFolderAsync()
        {
            return await StorageFolderHelpers.TryGetFolderFromPathAsync(Path, (exists) => Exists = exists);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}