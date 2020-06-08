using System;
using ApplicationState.Enumerations;
using PlexClient.Library.Models;

namespace ApplicationState
{
    public interface IApplicationStateService
    {
        IObservable<AppStateEnum> AppState { get; }
        IObservable<ArtistModel> Artist { get; }
        IObservable<AlbumModel> Album { get; }
        IObservable<int> MenuIndex { get; }

        void ToggleState();
        void PreviousMenu();
        void SelectArtist(ArtistModel artist);
        void SelectAlbum(AlbumModel album);
    }
}
