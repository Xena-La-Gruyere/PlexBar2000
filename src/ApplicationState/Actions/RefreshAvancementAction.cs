using Redux;

namespace ApplicationState.Actions
{
    public class RefreshAvancementAction : IAction
    {
        public double Avancement { get; }

        public RefreshAvancementAction(double avancement)
        {
            Avancement = avancement;
        }
    }
}
