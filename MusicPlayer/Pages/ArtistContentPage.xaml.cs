using ExtensionsLibrary.Extensions;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistContentPage : Page, IMusicPlayerPage, IScrollablePage
    {
        public ArtistContentPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"ArtistContentPage {GetHashCode()} Constructed");
#endif
        }

        ~ArtistContentPage()
        {
#if DEBUG
            Debug.WriteLine($"ArtistContentPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(ArtistContentPage), new PropertyMetadata(null));

        public ArtistModel Artist
        {
            get => (ArtistModel)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public static readonly DependencyProperty ArtistProperty = DependencyProperty.Register("Artist", typeof(ArtistModel), typeof(ArtistContentPage), new PropertyMetadata(null));

        public PageActions PageAction { get; set; }

        public object PageActionTarget { get; set; }

        private FrameworkElement HeaderElement;
        private Compositor ShadowCompositor;
        private ManipulationPropertySetReferenceNode ScrollProperties;
        private SpriteVisual SpriteVisual;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                PageAction = parameters.PageAction;
                PageActionTarget = parameters.PageActionTarget;
                Artist = parameters.PageParameter as ArtistModel;
            }
            base.OnNavigatedTo(e);
        }

        private void ArtistContentPage_Loaded(object sender, RoutedEventArgs e)
        {
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        ScrollInToView(PageActionTarget);
                        break;
                    }
            }
        }

        public bool ScrollInToView(object obj)
        {
            if (IsLoaded) ArtistContentListView.ScrollIntoView(obj ?? MusicPlayer.CurrentPlayingSong);
            return IsLoaded;
        }

        private void SongTileMinimalistic_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e as SongModel, Artist, PlayingLocation.Artist).FireAndForget();
        }

        private void ArtistHeader_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Artist).FireAndForget();
        }

        private void AlbumSmallHeader_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumSmallHeader_GoToAlbum(object sender, AlbumModel e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, e));
        }

        private void ArtistContentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (HeaderElement is FrameworkElement) HeaderElement.Margin = GetHeaderOffset();
            HeaderImage.Height = ActualWidth / 2.5;
        }

        private void ArtistSmallHeader_Loaded(object sender, RoutedEventArgs e)
        {
            HeaderElement = sender as FrameworkElement;
            HeaderElement.Margin = GetHeaderOffset();
            RenderDropShadow();
        }

        private Thickness GetHeaderOffset()
        {
            return new Thickness(20, (float)ActualWidth / 3, 20, -1);
        }

        private void ArtistContentListView_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = (VisualTreeHelper.GetChild(ArtistContentListView, 0) as Border).Child as ScrollViewer;
            ScrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer).GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            ShadowCompositor = ElementCompositionPreview.GetElementVisual(ShadowCanvas).Compositor;
            ElementCompositionPreview.GetElementVisual(HeaderImage).StartAnimation(AnimationConstants.OffsetY, ScrollProperties.Translation.Y * .7f);
        }

        private void RenderDropShadow()
        {
            SpriteVisual = ShadowCompositor.CreateSpriteVisual();
            DropShadow dropShadow = ShadowCompositor.CreateDropShadow();
            dropShadow.BlurRadius = 10f;
            dropShadow.Color = Windows.UI.Color.FromArgb(0xAA, 0, 0, 0);
            SpriteVisual.Shadow = dropShadow;
            VisualReferenceNode headerProperties = ElementCompositionPreview.GetElementVisual(HeaderElement).GetReference();
            VisualReferenceNode pageProperties = ElementCompositionPreview.GetElementVisual(this).GetReference();
            SpriteVisual.StartAnimation(AnimationConstants.OffsetY, ScrollProperties.Translation.Y + headerProperties.Offset.Y);
            SpriteVisual.StartAnimation(AnimationConstants.OffsetX, ScrollProperties.Translation.X + headerProperties.Offset.X);
            SpriteVisual.StartAnimation(AnimationConstants.SizeY, pageProperties.Size.Y + headerProperties.Offset.Y - ScrollProperties.Translation.Y);
            SpriteVisual.StartAnimation(AnimationConstants.SizeX, headerProperties.Size.X);
            ElementCompositionPreview.SetElementChildVisual(ShadowCanvas, SpriteVisual);
        }

        private void StopAnimations()
        {
            SpriteVisual.StopAnimation(AnimationConstants.OffsetX);
            SpriteVisual.StopAnimation(AnimationConstants.OffsetY);
            SpriteVisual.StopAnimation(AnimationConstants.SizeX);
            SpriteVisual.StopAnimation(AnimationConstants.SizeY);
            ElementCompositionPreview.GetElementVisual(HeaderElement).StopAnimation(AnimationConstants.OffsetY);
        }

        private void ArtistContentPage_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAnimations();
        }
    }
}