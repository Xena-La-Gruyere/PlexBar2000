using System;
using Redux;

namespace ApplicationState.Actions
{
    public class RefreshAvancementAction : IAction
    {
        public TimeSpan Avancement { get; }

        public RefreshAvancementAction(TimeSpan avancement)
        {
            Avancement = avancement;
        }
    }
}
