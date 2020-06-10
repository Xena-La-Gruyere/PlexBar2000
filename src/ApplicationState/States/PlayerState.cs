using PlexClient.Library.Models;

namespace ApplicationState.States
{
    public class PlayerState
    {
        public PlayerState()
        {
            PlayingTrack = null;
            PlayingState = PlayingStateEnum.Paused;
            Avancement = 0;
        }
        public PlayerState(TrackModel playingTrack, PlayingStateEnum playingState, double avancement)
        {
            PlayingTrack = playingTrack;
            PlayingState = playingState;
            Avancement = avancement;
        }

        public TrackModel PlayingTrack { get; }
        public PlayingStateEnum PlayingState { get; }
        public double Avancement { get; }

        public struct Builder
        {
            private readonly PlayerState _state;

            public TrackModel PlayingTrack;
            public PlayingStateEnum PlayingState;
            public double Avancement;

            public Builder(PlayerState state)
            {
                _state = state;

                PlayingTrack = state.PlayingTrack;
                PlayingState = state.PlayingState;
                Avancement = state.Avancement;
            }

            public bool Equal(PlayerState other)
            {
                return ReferenceEquals(PlayingTrack, other.PlayingTrack) &&
                       PlayingState == other.PlayingState &&
                       Avancement.Equals(other.Avancement);
            }

            public PlayerState Build()
            {
                if (Equal(_state)) return _state;

                return new PlayerState(PlayingTrack, PlayingState, Avancement);
            }
        }
    }
}
