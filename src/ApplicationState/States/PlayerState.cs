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
        public PlayerState(TrackModel playingTrack, PlayingStateEnum playingState, long avancement)
        {
            PlayingTrack = playingTrack;
            PlayingState = playingState;
            Avancement = avancement;
        }

        public TrackModel PlayingTrack { get; }
        public PlayingStateEnum PlayingState { get; }
        public long Avancement { get; }

        public struct Builder
        {
            private readonly PlayerState _state;

            public TrackModel PlayingTrack;
            public PlayingStateEnum PlayingState;
            public long Avancement;

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
                       Avancement == other.Avancement;
            }

            public PlayerState Build()
            {
                if (Equal(_state)) return _state;

                return new PlayerState(PlayingTrack, PlayingState, Avancement);
            }
        }
    }
}
