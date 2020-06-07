using BeefWebClient.Models;

namespace BeefWebClient.Parameters
{
    public class SetPlayerStateParameters
    {
        public double? Volume;
        public bool? IsMuted;
        public int? Position;
        public int? RelativePosition;
        public PlaybackState? PlaybackMode;
    }
}
