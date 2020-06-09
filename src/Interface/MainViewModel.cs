using System;
using System.Collections.Immutable;
using System.Windows.Input;
using ApplicationState;
using ApplicationState.Enumerations;
using Playlist;
using PlexClient.Library;
using PlexClient.Library.Models;
using ReactiveUI;

namespace Interface
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IApplicationStateService _applicationStateService;
        private readonly IPlexLibraryService _plexLibraryService;
        private readonly IPlaylistService _playlistService;

        public readonly IObservable<AppStateEnum> AppState;
        public readonly IObservable<ArtistModel[]> Artists;
        public readonly IObservable<ArtistModel> Artist;
        public readonly IObservable<AlbumModel> Album;
        public readonly IObservable<ImmutableArray<AlbumModel>> PlaylistAlbum;
        public readonly IObservable<char[]> Letters;
        public readonly IObservable<int> MenuIndex;

        public MainViewModel(
            IApplicationStateService applicationStateService,
            IPlexLibraryService plexLibraryService,
            IPlaylistService playlistService)
        {
            _applicationStateService = applicationStateService;
            _plexLibraryService = plexLibraryService;
            _playlistService = playlistService;
            AppState = applicationStateService.AppState;

            Artists = _plexLibraryService.Artists;
            Letters = _plexLibraryService.SearchLetters;
            MenuIndex = applicationStateService.MenuIndex;
            Artist = applicationStateService.Artist;
            Album = applicationStateService.Album;
            PlaylistAlbum = playlistService.Albums;

            // Initialize library
            _plexLibraryService.Initialize();
        }

        public void MiddleMouseClick(MouseButtonEventArgs args)
        {
            _applicationStateService.ToggleState();
        }

        public void ClickArtist(ArtistModel artist)
        {
            _applicationStateService.SelectArtist(artist);
        }

        public void ClickAlbum(AlbumModel album)
        {
            _applicationStateService.SelectAlbum(album);
        }

        public void ClickPrevious(MouseButtonEventArgs args)
        {
            _applicationStateService.PreviousMenu();
        }

        public void AddPlaylistAlbum(AlbumModel albumModel)
        {
            _playlistService.AddAlbum(albumModel);
        }

        public void HomeButton(MouseButtonEventArgs args)
        {
            _applicationStateService.HomeMenu();
        }

        public void PlaylistButton(MouseButtonEventArgs args)
        {
            _applicationStateService.PlaylistMenu();
        }
    }
}
