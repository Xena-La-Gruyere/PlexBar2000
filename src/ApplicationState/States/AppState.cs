using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationState.Enumerations;

namespace ApplicationState.States
{
    public class AppState
    {
        public AppStateEnum State { get; }
        public AppState(AppStateEnum state)
        {
            State = state;
        }

        public struct Builder
        {
            private readonly AppState _state;

            public AppStateEnum State;

            public Builder(AppState state)
            {
                _state = state;

                State = state.State;
            }

            public bool Equals(AppState other)
            {
                return State == other.State;
            }

            public AppState Build()
            {
                if (Equals(_state)) return _state;

                return new AppState(State);
            }
        }

    }
}
