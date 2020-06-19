using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ApplicationState.Actions;
using ApplicationState.Enumerations;
using ApplicationState.Reducers;
using ApplicationState.States;
using Library.Abstractions;
using Library.Abstractions.Models;
using Redux;

namespace ApplicationState
{
    public class ApplicationStateService : IApplicationStateService
    {
        private readonly ILibraryService _service;
        private readonly IStore<AppState> _store;
        public IObservable<AppStateEnum> AppState { get; }
        public IObservable<ArtistModel> Artist { get; }
        public IObservable<AlbumModel> Album { get; }
        public IObservable<MenuStateEnum> MenuIndex { get; }
        public IObservable<ImmutableArray<ArtistModel>> Artists { get; }
        public IObservable<ImmutableArray<AlbumModel>> Playlist { get; }
        public IObservable<ImmutableArray<char>> SearchLetters { get; }
        public IObservable<PlayerState> PlayerState { get; }
        public IObservable<TrackModel> PlayingTrack { get; }

        public ApplicationStateService(ILibraryService service)
        {
            _service = service;
            _store = new Store<AppState>(AppStateReducer.Execute, new AppState());

            var appStateConn = _store.Select(s => s.State)
                .DistinctUntilChanged()
                .Replay(1);
            AppState = appStateConn;

            var artistConn = _store.Select(s => s.Artist)
                .DistinctUntilChanged()
                .Replay(1);
            Artist = artistConn;

            var albumConn = _store.Select(s => s.Album)
                .DistinctUntilChanged()
                .Replay(1);
            Album = albumConn;

            var menuIndexConn = _store.Select(s => s.MenuIndex)
                .DistinctUntilChanged()
                .Replay(1);
            MenuIndex = menuIndexConn;

            var artistsConn = _store.Select(s => s.Artists)
                .DistinctUntilChanged()
                .Replay(1);
            Artists = artistsConn;

            var playlistConn = _store.Select(s => s.PlayerState.Playlist)
                .DistinctUntilChanged()
                .Replay(1);
            Playlist = playlistConn;

            var searchLettersConn = _store.Select(s => s.SearchLetters)
                .DistinctUntilChanged()
                .Replay(1);
            SearchLetters = searchLettersConn;

            var playerStateConn = _store.Select(s => s.PlayerState)
                .DistinctUntilChanged()
                .Replay(1);
            PlayerState = playerStateConn;

            var playingTrackConn = _store
                .Select(s => s.PlayerState.PlayingTrack)
                .DistinctUntilChanged()
                .Replay(1);
            PlayingTrack = playingTrackConn;

            appStateConn.Connect();
            artistConn.Connect();
            albumConn.Connect();
            menuIndexConn.Connect();
            artistsConn.Connect();
            playlistConn.Connect();
            searchLettersConn.Connect();
            playerStateConn.Connect();
            playingTrackConn.Connect();
        }

        public void ToggleState()
        {
            _store.Dispatch(new ToggleGlobalState());
        }

        public void PreviousMenu()
        {
            _store.Dispatch(new PreviousMenu());
        }

        public void HomeMenu()
        {
            _store.Dispatch(new HomeMenu());
        }

        public void PlaylistMenu()
        {
            _store.Dispatch(new PlaylistMenu());
        }

        public void SelectArtist(ArtistModel artist)
        {
            _store.Dispatch(new SelectArtist(artist));

            _service.GetArtist(artist).ContinueWith(task => 
                _store.Dispatch(new SelectArtist(task.Result)), 
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void SelectAlbum(AlbumModel album)
        {
            _store.Dispatch(new SelectAlbum(album));

            _service.GetAlbum(album).ContinueWith(task =>
                _store.Dispatch(new SelectAlbum(task.Result)),
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void PlayAlbum(AlbumModel album)
        {
            _service.GetAlbum(album).ContinueWith(task =>
                {
                    _store.Dispatch(new PlayAlbumAction(task.Result));
                },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void PlayTrackInPlaylist(TrackModel track)
        {
            _store.Dispatch(new PlayTrackInPlaylistAction(track));
        }

        public void PlayTrackAndAlbum(AlbumModel album, TrackModel track)
        {
            _store.Dispatch(new PlayTrackAndAlbumAction(album, track));
        }

        public void AddPlaylistAlbum(AlbumModel album)
        {
            // Get album from plex
            _service.GetAlbum(album).ContinueWith(task =>
                {
                    // Add to playlist
                    _store.Dispatch(new AddAlbumPlaylistAction(task.Result));
                },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void RemovePlaylistAlbum(AlbumModel album)
        {
            _store.Dispatch(new RemoveAlbumPlaylistAction(album));
        }

        public void ClearPlaylist()
        {
            _store.Dispatch(new ClearPlaylistAction());
        }

        public void LoadArtists()
        {
            // Get artists from plex
            _service.GetArtists().ContinueWith(task =>
                {
                    // Add to playlist
                    _store.Dispatch(new ArtistsLoaded(task.Result.ToImmutableArray()));
                },
                TaskScheduler.FromCurrentSynchronizationContext());
            _store.Dispatch(new ClearPlaylistAction());
        }

        public void PauseResume()
        {
            _store.Dispatch(new PauseResumeAction());
        }

        public void ActualAvancement(TimeSpan avancement)
        {
            _store.Dispatch(new RefreshAvancementAction(avancement));
        }

        public void ActualFormat(int channels, int sampleRate)
        {
            _store.Dispatch(new ChangeWaveFormatAction(channels, sampleRate));
        }

        public void Next()
        {
            _store.Dispatch(new PlayNextAction());
        }

        public void Previous()
        {
            _store.Dispatch(new PlayPreviousAction());
        }

        public void UpVolume()
        {
            _store.Dispatch(new UpVolumeAction(5));
        }

        public void DownVolume()
        {
            _store.Dispatch(new DownVolumeAction(5));
        }

        public void Stop()
        {
            _store.Dispatch(new StopAction());
        }
    }
}
