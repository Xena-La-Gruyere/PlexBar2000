using Redux;

namespace ApplicationState.Actions
{
    public class DownVolumeAction : IAction
    {
        public int Amount { get; }

        public DownVolumeAction(int amount)
        {
            Amount = amount;
        }
    }
}
