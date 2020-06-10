using System;
using ApplicationState.Enumerations;
using ApplicationState.States;
using PlexClient.Library.Models;

namespace ApplicationState
{
    public interface IApplicationStateService
    {
        IObservable<AppStateEnum> AppState { get; }
        IObservable<ArtistModel> Artist { get; }
        IObservable<AlbumModel> Album { get; }
        IObservable<MenuStateEnum> MenuIndex { get; }

        void ToggleState();
        void PreviousMenu();
        void HomeMenu();
        void PlaylistMenu();
        void SelectArtist(ArtistModel artist);
        void SelectAlbum(AlbumModel album);
    }
}
