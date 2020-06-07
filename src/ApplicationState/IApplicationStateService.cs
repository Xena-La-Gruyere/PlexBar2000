using System;
using ApplicationState.Enumerations;

namespace ApplicationState
{
    public interface IApplicationStateService
    {
        IObservable<AppStateEnum> AppState { get; }

        void ToggleState();
    }
}
