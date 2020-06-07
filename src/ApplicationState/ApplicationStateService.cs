using System;
using System.Reactive.Linq;
using ApplicationState.Actions;
using ApplicationState.Enumerations;
using ApplicationState.Reducers;
using ApplicationState.States;
using PlexClient.Library;
using PlexClient.Library.Models;
using Redux;

namespace ApplicationState
{
    public class ApplicationStateService : IApplicationStateService
    {
        private readonly IPlexLibraryService _plexService;
        private readonly IStore<AppState> _store;
        public IObservable<AppStateEnum> AppState { get; }
        public IObservable<ArtistModel> Artist { get; }
        public IObservable<int> MenuIndex { get; }

        public ApplicationStateService(IPlexLibraryService plexService)
        {
            _plexService = plexService;
            _store = new Store<AppState>(AppStateReducer.Execute, new AppState());

            _plexService.Artist.DistinctUntilChanged()
                .Where(a => a != null)
                .Subscribe(artist => _store.Dispatch(new SelectArtist(artist)));

            var appStateConn = _store.Select(s => s.State)
                .DistinctUntilChanged()
                .Replay(1);
            AppState = appStateConn;

            var artistConn = _store.Select(s => s.Artist)
                .DistinctUntilChanged()
                .Replay(1);
            Artist = artistConn;

            var menuIndexConn = _store.Select(s => s.MenuIndex)
                .DistinctUntilChanged()
                .Replay(1);
            MenuIndex = menuIndexConn;

            appStateConn.Connect();
            artistConn.Connect();
            menuIndexConn.Connect();
        }

        public void ToggleState()
        {
            _store.Dispatch(new ToggleGlobalState());
        }

        public void PreviousMenu()
        {
            _store.Dispatch(new PreviousMenu());
        }

        public void SelectArtist(ArtistModel artist)
        {
            _store.Dispatch(new SelectArtist(artist));

            _plexService.GetArtist(artist);
        }
    }
}
