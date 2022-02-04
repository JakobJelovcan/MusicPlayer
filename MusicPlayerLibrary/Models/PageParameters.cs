using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.MusicPlayer;
using System;

namespace MusicPlayerLibrary.Models
{
    public class PageParameters
    {
        public MusicPlayerModel MusicPlayer { get; private set; }

        public object PageParameter { get; private set; }

        public PageActions PageAction { get; private set; }

        public object PageActionTarget { get; private set; }

        public Type NavigateToPage { get; private set; }

        public PageParameters NavigateToPageParameters { get; private set; }

        public PageParameters(MusicPlayerModel musicPlayer)
        {
            MusicPlayer = musicPlayer;
            PageParameter = musicPlayer;
            PageAction = default;
            PageActionTarget = default;
        }

        public PageParameters(MusicPlayerModel musicPlayer, object pageParameter)
        {
            MusicPlayer = musicPlayer;
            PageParameter = pageParameter ?? musicPlayer;
            PageAction = default;
            PageActionTarget = pageParameter;
        }

        public PageParameters(MusicPlayerModel musicPlayer, object pageParameter, PageActions pageAction)
        {
            MusicPlayer = musicPlayer;
            PageParameter = pageParameter ?? musicPlayer;
            PageAction = pageAction;
            PageActionTarget = pageParameter;
        }

        public PageParameters(MusicPlayerModel musicPlayer, PageActions pageAction, Type navigateToPage, PageParameters navigateToPageParameters)
        {
            MusicPlayer = musicPlayer;
            PageParameter = musicPlayer;
            PageAction = pageAction;
            NavigateToPage = navigateToPage;
            PageActionTarget = default;
            NavigateToPageParameters = navigateToPageParameters;
        }

        public PageParameters(MusicPlayerModel musicPlayer, object pageParameter, PageActions pageAction, object pageActionTarget)
        {
            MusicPlayer = musicPlayer;
            PageParameter = pageParameter ?? musicPlayer;
            PageAction = pageAction;
            PageActionTarget = pageActionTarget;
        }
    }
}