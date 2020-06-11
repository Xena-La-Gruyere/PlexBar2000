using Redux;

namespace ApplicationState.Actions
{
    public class UpVolumeAction : IAction
    {
        public int Amount { get; }

        public UpVolumeAction(int amount)
        {
            Amount = amount;
        }
    }
}
