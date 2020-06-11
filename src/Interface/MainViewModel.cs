using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Windows.Input;
using ApplicationState;
using ApplicationState.Enumerations;
using ApplicationState.States;
using PlexClient.Library.Models;
using ReactiveUI;

namespace Interface
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IApplicationStateService _applicationStateService;

        public readonly IObservable<AppStateEnum> AppState;
        public readonly IObservable<ImmutableArray<ArtistModel>> Artists;
        public readonly IObservable<ArtistModel> Artist;
        public readonly IObservable<AlbumModel> Album;
        public readonly IObservable<ImmutableArray<AlbumModel>> PlaylistAlbum;
        public readonly IObservable<ImmutableArray<char>> Letters;
        public readonly IObservable<int> MenuIndex;
        public readonly IObservable<PlayerState> Player;

        public MainViewModel(IApplicationStateService applicationStateService)
        {
            _applicationStateService = applicationStateService;
            AppState = applicationStateService.AppState;

            Artists = applicationStateService.Artists;
            Letters = applicationStateService.SearchLetters;
            MenuIndex = applicationStateService.MenuIndex.Select(e => (int)e);
            Artist = applicationStateService.Artist;
            Album = applicationStateService.Album;
            PlaylistAlbum = applicationStateService.Playlist;
            Player = applicationStateService.PlayerState;

            // Initialize library
            applicationStateService.LoadArtists();
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
            _applicationStateService.AddPlaylistAlbum(albumModel);
        }
        public void PlayAlbum(AlbumModel albumModel)
        {
            _applicationStateService.PlayAlbum(albumModel);
        }
        public void HomeButton(MouseButtonEventArgs args)
        {
            _applicationStateService.HomeMenu();
        }

        public void PlaylistButton(MouseButtonEventArgs args)
        {
            _applicationStateService.PlaylistMenu();
        }
        public void RemoveAlbumButton(AlbumModel albumModel)
        {
            _applicationStateService.RemovePlaylistAlbum(albumModel);
        }
        public void PauseResume(MouseButtonEventArgs args)
        {
            _applicationStateService.PauseResume();
        }
        public void WheelUp(MouseWheelEventArgs args)
        {
            _applicationStateService.UpVolume();
        }
        public void WheelDown(MouseWheelEventArgs args)
        {
            _applicationStateService.DownVolume();
        }
    }
}
