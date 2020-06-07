using System;
using System.Windows.Input;
using ApplicationState;
using ApplicationState.Enumerations;
using PlexClient.Client.Models;
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
        public readonly IObservable<char[]> Letters;

        public MainViewModel(IApplicationStateService applicationStateService,
            IPlexLibraryService plexLibraryService)
        {
            _applicationStateService = applicationStateService;
            _plexLibraryService = plexLibraryService;
            AppState = applicationStateService.AppState;

            Artists = _plexLibraryService.Artists;
            Letters = _plexLibraryService.SearchLetters;

            // Initialize library
            _plexLibraryService.Initialize();
        }

        public void MiddleMouseClick(MouseButtonEventArgs args)
        {
            _applicationStateService.ToggleState();
        }
    }
}
