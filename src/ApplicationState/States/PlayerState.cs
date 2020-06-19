using System;
using System.Collections.Immutable;

namespace ApplicationState.States
{
    public class PlayerState
    {
        public PlayerState(int volumentPercentage)
        {
            VolumentPercentage = volumentPercentage;
            PlayingTrack = null;
            Playlist = ImmutableArray<AlbumModel>.Empty;
            PlayingTrackInd = 0;
            PlayingState = PlayingStateEnum.Stopped;
            Avancement = TimeSpan.Zero;
        }

        public PlayerState(int playingTrackInd, PlayingStateEnum playingState, TimeSpan avancement, int volumentPercentage, ImmutableArray<AlbumModel> playlist, TrackModel playingTrack)
        {
            PlayingTrackInd = playingTrackInd;
            PlayingState = playingState;
            Avancement = avancement;
            VolumentPercentage = volumentPercentage;
            Playlist = playlist;
            PlayingTrack = playingTrack;
        }

        public ImmutableArray<AlbumModel> Playlist { get; }
        public int PlayingTrackInd { get; }
        public TrackModel PlayingTrack { get; }
        public PlayingStateEnum PlayingState { get; }
        public TimeSpan Avancement { get; }
        public int VolumentPercentage { get; }

        public struct Builder
        {
            private readonly PlayerState _state;

            public int PlayingTrackInd;
            public PlayingStateEnum PlayingState;
            public TimeSpan Avancement;
            public int VolumentPercentage;
            public ImmutableArray<AlbumModel> Playlist;
            public TrackModel PlayingTrack;

            public Builder(PlayerState state)
            {
                _state = state;

                PlayingTrackInd = state.PlayingTrackInd;
                PlayingState = state.PlayingState;
                Avancement = state.Avancement;
                VolumentPercentage = state.VolumentPercentage;
                Playlist = state.Playlist;
                PlayingTrack = state.PlayingTrack;
            }

            public bool Equal(PlayerState other)
            {
                return PlayingTrackInd == other.PlayingTrackInd &&
                       PlayingState == other.PlayingState &&
                       VolumentPercentage == other.VolumentPercentage &&
                       Playlist == other.Playlist &&
                       ReferenceEquals(PlayingTrack, other.PlayingTrack) &&
                       Avancement.Equals(other.Avancement);
            }

            public PlayerState Build()
            {
                if (Equal(_state)) return _state;

                return new PlayerState(PlayingTrackInd, PlayingState, Avancement, VolumentPercentage, Playlist, PlayingTrack);
            }
        }
    }
}
