using Redux;

namespace ApplicationState.Actions
{
    public class ChangeWaveFormatAction : IAction
    {
        public int Channels { get; }
        public int SampleRate { get; }

        public ChangeWaveFormatAction(int channels, int sampleRate)
        {
            Channels = channels;
            SampleRate = sampleRate;
        }
    }
}
