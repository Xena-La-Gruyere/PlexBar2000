using System;
using System.Collections.Immutable;
using PlexClient.Library.Models;

namespace ApplicationState.States
{
    public class PlayerState
    {
        public PlayerState(int volumentPercentage)
        {
            VolumentPercentage = volumentPercentage;
            Playlist = ImmutableArray<AlbumModel>.Empty;
            PlayingTrack = 0;
            PlayingState = PlayingStateEnum.Paused;
            Avancement = TimeSpan.Zero;
        }

        public PlayerState(int playingTrack, PlayingStateEnum playingState, TimeSpan avancement, int volumentPercentage, ImmutableArray<AlbumModel> playlist)
        {
            PlayingTrack = playingTrack;
            PlayingState = playingState;
            Avancement = avancement;
            VolumentPercentage = volumentPercentage;
            Playlist = playlist;
        }

        public ImmutableArray<AlbumModel> Playlist { get; }
        public int PlayingTrack { get; }
        public PlayingStateEnum PlayingState { get; }
        public TimeSpan Avancement { get; }
        public int VolumentPercentage { get; }

        public struct Builder
        {
            private readonly PlayerState _state;

            public int PlayingTrack;
            public PlayingStateEnum PlayingState;
            public TimeSpan Avancement;
            public int VolumentPercentage;
            public ImmutableArray<AlbumModel> Playlist;

            public Builder(PlayerState state)
            {
                _state = state;

                PlayingTrack = state.PlayingTrack;
                PlayingState = state.PlayingState;
                Avancement = state.Avancement;
                VolumentPercentage = state.VolumentPercentage;
                Playlist = state.Playlist;
            }

            public bool Equal(PlayerState other)
            {
                return PlayingTrack == other.PlayingTrack &&
                       PlayingState == other.PlayingState &&
                       VolumentPercentage == other.VolumentPercentage &&
                       Playlist == other.Playlist &&
                       Avancement.Equals(other.Avancement);
            }

            public PlayerState Build()
            {
                if (Equal(_state)) return _state;

                return new PlayerState(PlayingTrack, PlayingState, Avancement, VolumentPercentage, Playlist);
            }
        }
    }
}
