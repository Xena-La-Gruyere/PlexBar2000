using System;
using System.Reactive.Linq;
using ApplicationState.Actions;
using ApplicationState.Enumerations;
using ApplicationState.Reducers;
using ApplicationState.States;
using Redux;

namespace ApplicationState
{
    public class ApplicationStateService : IApplicationStateService
    {
        private readonly IStore<AppState> _store;
        public IObservable<AppStateEnum> AppState { get; }

        public ApplicationStateService()
        {
            _store = new Store<AppState>(AppStateReducer.Execute, new AppState(AppStateEnum.Explorer));

            var appStateConn = _store.Select(s => s.State)
                .DistinctUntilChanged()
                .Replay(1);
            AppState = appStateConn;

            appStateConn.Connect();
        }
        public void ToggleState()
        {
            _store.Dispatch(new ToggleGlobalState());
        }
    }
}
