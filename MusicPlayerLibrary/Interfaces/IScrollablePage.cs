using MusicPlayerLibrary.Constants;
namespace MusicPlayerLibrary.Interfaces
{
    public interface IScrollablePage
    {
        public bool ScrollInToView(object obj);

        public PageActions PageAction { get; set; }

        public object PageActionTarget { get; set; }
    }
}