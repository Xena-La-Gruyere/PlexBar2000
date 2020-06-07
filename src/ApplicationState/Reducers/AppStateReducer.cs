using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationState.Actions;
using ApplicationState.States;
using Redux;

namespace ApplicationState.Reducers
{
    public static class AppStateReducer
    {
        public static AppState Execute(AppState state, IAction action)
        {
            var builder = new AppState.Builder(state);

            switch (action)
            {
                case ChangeGlobalState change:
                    builder.State = change.State;
                    break;
            }

            return builder.Build();
        }
    }
}
