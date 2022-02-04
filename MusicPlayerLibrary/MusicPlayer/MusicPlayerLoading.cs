using ExtensionsLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Collections;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.DataProperties;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Info;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace MusicPlayerLibrary.MusicPlayer
{
    public partial class MusicPlayerModel
    {
        //
        //LoadData
        //
        public async Task LoadDataFromStorageAsync(bool removeMissingSongs = true)
        {
            InfoMessage.ShowMessage("Your content is being loaded in the background.", InfoTileSeverity.Informational, true);
            await LoadDataFromStorageAsync(DBAccess.StorageFolders, removeMissingSongs);
        }

        public async Task LoadDataFromStorageAsync(IEnumerable<StorageFolderModel> storageFolderModels, bool removeMissingSongs = true)
        {
            ConcurrentBag<StorageFolder> storageFolders = new ConcurrentBag<StorageFolder> { KnownFolders.MusicLibrary };
            await Task.WhenAll(storageFolderModels.DistinctBy(S => S.Path).Select(async F => storageFolders.Add(await F.TryGetFolderAsync())));
            await LoadDataFromSongFilesAsync(await StorageFolderHelpers.GetSongFilesFromFoldersAsync(storageFolders), removeMissingSongs);
        }

        private async Task<List<SongModel>> LoadDataFromSongFilesAsync(IEnumerable<StorageFile> songFiles, bool isSaveEnabled = true, bool removeMissingSongs = true)
        {
            IEnumerable<SongProperties> songsProperties = (await SongProperties.LoadPropertiesAsync(songFiles.Where(SF => !Songs.Any(S => S.Path == SF.Path)))).DistinctBy(S => new { S.Title, S.Album, S.Artist });
            IEnumerable<AlbumProperties> albumsProperties = AlbumProperties.LoadProperties(songsProperties.Where(S => !Albums.Any(A => A.Album == S.Album && A.Artist == S.Artist)).DistinctBy(S => S.Album));
            IEnumerable<ArtistProperties> artistsProperties = ArtistProperties.LoadProperties(albumsProperties.Where(Al => !Artists.Any(Ar => Ar.Artist == Al.Artist)).DistinctBy(Al => Al.Artist));
            artistsProperties.ForEach(A => A.ToArtistModel(this, isSaveEnabled));
            albumsProperties.ForEach(A => A.ToAlbumModel(Artists.GetParentArtist(A), isSaveEnabled));
            songsProperties.ForEach(S => S.ToSongModel(Albums.GetParentAlbum(S), isSaveEnabled));
            DBAccess.Songs.AddOrIgnoreRange(Songs);
            DBAccess.Albums.AddOrIgnoreRange(Albums);
            DBAccess.Artists.AddOrIgnoreRange(Artists);
            await DBAccess.SaveChangesAsync();
            if (removeMissingSongs) RemoveMissingSongs(songFiles);
            return Songs.Where(S => songFiles.Any(SF => SF.Path == S.Path)).ToList();
        }

        private void RemoveMissingSongs(IEnumerable<StorageFile> songFiles)
        {
            Songs.Where(S => !songFiles.Any(F => F.Path == S.Path)).ToArray().ForEach(S => S.Remove());
        }
        //
        //LastPlayed
        //
        private ObservableCollection<BaseMusicModel> LoadLastPlayed()
        {
            return Artists.Concatinate(Albums).Concatinate(Playlists).Where(M => M.LastPlayed > 0).OrderByDescending(M => M.LastPlayed).Take(Settings.NumOfLastPlayedItems).ToObservableCollection();
        }

        private void AddToLastPlayed(BaseMusicModel musicModel)
        {
            if (LastPlayed.FirstOrDefault() != musicModel)
            {
                LastPlayed.Remove(musicModel);
                LastPlayed.Insert(0, musicModel);
                TruncateLastPlayed();
            }
        }

        private void TruncateLastPlayed()
        {
            if (LastPlayed.Count > Settings.NumOfLastPlayedItems) LastPlayed.Remove(Settings.NumOfLastPlayedItems);
        }
        //
        //MostPlayed
        //
        private ObservableCollection<BaseMusicModel> LoadMostPlayed()
        {
            return Artists.Concatinate(Albums).Concatinate(Playlists).Where(M => M.TimesPlayed > 0).OrderByDescending(M => M.TimesPlayed).Take(Settings.NumOfMostPlayedItems).ToObservableCollection();
        }

        private void AddToMostPlayed(BaseMusicModel musicModel)
        {
            if (!MostPlayed.Any()) MostPlayed.Add(musicModel);
            for (int i = 0; i < MostPlayed.Count; i++)
            {
                if (MostPlayed[i].TimesPlayed <= musicModel.TimesPlayed)
                {
                    if (MostPlayed[i] != musicModel)
                    {
                        MostPlayed.Remove(musicModel);
                        MostPlayed.Insert(i, musicModel);
                    }
                    break;
                }
            }
            TruncateMostPlayed();
        }

        private void TruncateMostPlayed()
        {
            if (MostPlayed.Count > Settings.NumOfMostPlayedItems) MostPlayed.Remove(Settings.NumOfMostPlayedItems);
        }
        //
        //ForYou
        //
        private ObservableCollection<AlbumModel> LoadAlbumsForYou()
        {
            IEnumerable<GenreModel> genres = Genres.OrderByDescending(G => G.TimesPlayed).Take((int)Settings.NumOfGenresForYou);
            ObservableCollection<AlbumModel> albums = new ObservableCollection<AlbumModel>();
            foreach (GenreModel genre in genres) albums.AddRange(Albums.Where(A => A.Genres.Any(G => G?.Equals(genre) ?? false))?.Take((int)Settings.NumOfAlbumsPerGenre));
            return albums;
        }

        private ObservableCollection<SongModel> LoadSongsForYou()
        {
            IEnumerable<GenreModel> genres = Genres.OrderByDescending(G => G.TimesPlayed).Take((int)Settings.NumOfGenresForYou);
            ObservableCollection<SongModel> songs = new ObservableCollection<SongModel>();
            foreach (GenreModel genre in genres) songs.AddRange(Songs.Where(S => S.Genre?.Equals(genre) ?? false)?.Take((int)Settings.NumOfSongsPerGenre));
            return songs;
        }
        //
        //Initialize
        //
        public async Task InitializeAsync(object args)
        {
            ApplicationTheme = Settings.ApplicationTheme;
            MediaPlayer.Volume = Settings.Volume;
            MediaPlayer.IsMuted = Settings.Muted;
            Artists = DBAccess.Artists.Include(A => A.Image).Include(A => A.LargeImage).Include(A => A.Songs).Include(A => A.Albums).ForEach(A => A.Parent = this).OrderBy(A => A.Artist).ToObservableCollection();
            Albums = DBAccess.Albums.Include(A => A.Image).Include(A => A.Songs).OrderBy(A => A.Album).ToObservableCollection();
            Songs = DBAccess.Songs.Include(S => S.Image).Include(S => S.Lyrics).Include(S => S.PlaylistSongLinks).OrderBy(S => S.Title.ToLower()).ToObservableCollection();
            Playlists = DBAccess.Playlists.Include(I => I.Image).Include(P => P.SongLinks).ForEach(P => P.Parent = this).OrderBy(P => P.Playlist).ToObservableCollection();
            Genres = DBAccess.Genres.ToObservableCollection();
            Parallel.ForEach(Albums, A => A.Songs = A.Songs.AsEnumerable().OrderBy(S => S.Track).ToObservableCollection());
            Parallel.ForEach(Artists, A => { A.Albums = A.Albums.AsEnumerable().OrderBy(A => A.Year).ToList(); A.Songs = A.Songs.AsEnumerable().OrderByDescending(S => S.ParentAlbum.Year).ThenBy(S => S.Track).ToList(); A.GroupedContent = new ObservableGroupedCollection<AlbumModel, SongModel>(A.Songs.GroupBy(S => S.ParentAlbum)); });
            Parallel.ForEach(Playlists, P => P.SongLinks = P.SongLinks.AsEnumerable().OrderBy(SL => SL.Song.Title).ToObservableCollection());
            LastPlayed = LoadLastPlayed();
            MostPlayed = LoadMostPlayed();
            AlbumsForYou = LoadAlbumsForYou();
            SongsForYou = LoadSongsForYou();
            CurrentPlayingQueue = new Queue<SongModel>();
            if (Settings.FirstBoot || !File.Exists(StorageConstants.DBPath))
            {
                Settings.FirstBoot = false;
                await LoadDataFromStorageAsync();
                InfoMessage.ShowMessage("All content has been successfuly loaded.", InfoTileSeverity.Success, true);
            }
            if (args is FileActivatedEventArgs fileArgs && fileArgs.Files.Any())
            {
                PlayMusicModel(PlayingLocation.OpenWith, await LoadDataFromSongFilesAsync(fileArgs.Files.Where(F => F.IsOfType(StorageItemTypes.File)).Select(F => F as StorageFile), Settings.SaveOpenWithContent, false)).FireAndForget();
                Songs.Where(S => S.PlayingState != PlayingState.NotPlaying).ForEach(S => S.PlayingState = PlayingState.NotPlaying);
                Albums.Where(A => A.PlayingState != PlayingState.NotPlaying).ForEach(A => A.PlayingState = PlayingState.NotPlaying);
                Artists.Where(A => A.PlayingState != PlayingState.NotPlaying).ForEach(A => A.PlayingState = PlayingState.NotPlaying);
                Playlists.Where(P => P.PlayingState != PlayingState.NotPlaying).ForEach(P => P.PlayingState = PlayingState.NotPlaying);
            }
            else
            {
                SongModel lastPlayingSong = Songs.FirstOrDefault(S => S.PlayingState != PlayingState.NotPlaying);
                BaseMusicModel lastPlayingContent = Artists.FirstOrDefault(A => A.PlayingState != PlayingState.NotPlaying) ?? Albums.FirstOrDefault(A => A.PlayingState != PlayingState.NotPlaying) ?? (BaseMusicModel)Playlists.FirstOrDefault(P => P.PlayingState != PlayingState.NotPlaying);
                if (lastPlayingSong != null)
                {
                    if (lastPlayingContent == null) PlayMusicModel(lastPlayingSong, PlayingLocation.Songs, Songs.ToList(), false, true).FireAndForget();
                    else PlayMusicModel(lastPlayingSong, lastPlayingContent, lastPlayingContent.GetPlayingLocation(), false, true).FireAndForget();
                }
            }
        }
    }
}