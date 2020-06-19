using System;
using System.Collections.Immutable;
using ApplicationState.Enumerations;
using ApplicationState.States;
using Library.Abstractions.Models;

namespace ApplicationState
{
    public interface IApplicationStateService
    {
        IObservable<AppStateEnum> AppState { get; }
        IObservable<ArtistModel> Artist { get; }
        IObservable<AlbumModel> Album { get; }
        IObservable<MenuStateEnum> MenuIndex { get; }
        IObservable<ImmutableArray<ArtistModel>> Artists { get; }
        IObservable<ImmutableArray<AlbumModel>> Playlist { get; }
        IObservable<ImmutableArray<char>> SearchLetters { get; }
        IObservable<PlayerState> PlayerState { get; }
        IObservable<TrackModel> PlayingTrack { get; }

        void ToggleState();
        void PreviousMenu();
        void HomeMenu();
        void PlaylistMenu();
        void SelectArtist(ArtistModel artist);
        void SelectAlbum(AlbumModel album);
        void PlayAlbum(AlbumModel album);
        void PlayTrackInPlaylist(TrackModel track);
        void PlayTrackAndAlbum(AlbumModel album, TrackModel track);
        void AddPlaylistAlbum(AlbumModel album);
        void RemovePlaylistAlbum(AlbumModel album);
        void ClearPlaylist();
        void LoadArtists();
        void PauseResume();
        void ActualAvancement(TimeSpan avancement);
        void Stop();
        void Next();
        void Previous();
        void UpVolume();
        void DownVolume();
    }
}
