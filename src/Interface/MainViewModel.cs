using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ApplicationState;
using ApplicationState.Enumerations;
using ApplicationState.States;
using Library.Abstractions.Models;
using ReactiveUI;
using ThemeColorManager;

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
        public readonly IObservable<TrackModel> PlayingTrack;
        public readonly IObservable<Brush> Background;
        public readonly IObservable<BitmapSource> Thumbnail;
        public readonly IObservable<Brush> Primary;
        public readonly IObservable<Brush> Text;
        public readonly IObservable<Brush> TextInformation;

        public MainViewModel(IApplicationStateService applicationStateService, IImageTheme imageTheme)
        {
            _applicationStateService = applicationStateService;
            AppState = applicationStateService.AppState;

            Artists = applicationStateService.Artists.Where(a => a.Any());
            Letters = applicationStateService.SearchLetters;
            MenuIndex = applicationStateService.MenuIndex.Select(e => (int)e);
            Artist = applicationStateService.Artist;
            Album = applicationStateService.Album;
            PlaylistAlbum = applicationStateService.Playlist;
            Player = applicationStateService.PlayerState;
            PlayingTrack = applicationStateService.PlayingTrack;
            Background = imageTheme.Background;
            Thumbnail = imageTheme.Image;
            Primary = imageTheme.Primary;
            Text = imageTheme.Text;
            TextInformation = imageTheme.TextInformation;

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
        public void PauseResume()
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
        public void PlayTrackInPlaylist(TrackModel track)
        {
            _applicationStateService.PlayTrackInPlaylist(track);
        }
        public void PlayTrackAndAlbum(AlbumModel album, TrackModel track)
        {
            _applicationStateService.PlayTrackAndAlbum(album, track);
        }
        public void PlayNext()
        {
            _applicationStateService.Next();
        }
        public void PlayPrevious()
        {
            _applicationStateService.Previous();
        }
        public void ClearPlaylist()
        {
            _applicationStateService.ClearPlaylist();
        }
    }
}
