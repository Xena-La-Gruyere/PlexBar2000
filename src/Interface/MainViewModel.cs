using System;
using System.Windows.Input;
using ApplicationState;
using ApplicationState.Enumerations;
using PlexClient.Library;
using PlexClient.Library.Models;
using ReactiveUI;

namespace Interface
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IApplicationStateService _applicationStateService;
        private readonly IPlexLibraryService _plexLibraryService;

        public readonly IObservable<AppStateEnum> AppState;
        public readonly IObservable<ArtistModel[]> Artists;
        public readonly IObservable<ArtistModel> Artist;
        public readonly IObservable<AlbumModel> Album;
        public readonly IObservable<char[]> Letters;
        public readonly IObservable<int> MenuIndex;

        public MainViewModel(IApplicationStateService applicationStateService,
            IPlexLibraryService plexLibraryService)
        {
            _applicationStateService = applicationStateService;
            _plexLibraryService = plexLibraryService;
            AppState = applicationStateService.AppState;

            Artists = _plexLibraryService.Artists;
            Letters = _plexLibraryService.SearchLetters;
            MenuIndex = applicationStateService.MenuIndex;
            Artist = applicationStateService.Artist;
            Album = applicationStateService.Album;

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
    }
}
