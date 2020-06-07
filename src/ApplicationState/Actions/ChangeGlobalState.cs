using ApplicationState.Enumerations;
using Redux;

namespace ApplicationState.Actions
{
    public class ChangeGlobalState : IAction
    {
        public ChangeGlobalState(AppStateEnum state)
        {
            State = state;
        }

        public AppStateEnum State { get; private set; }
    }
}
