using System.Collections.Immutable;
using ApplicationState.Enumerations;

namespace ApplicationState.States
{
    public class AppState
    {
        public AppStateEnum State { get; }
        public MenuStateEnum MenuIndex { get; }
        public ArtistModel Artist { get; }
        public AlbumModel Album { get; }
        public ImmutableArray<ArtistModel> Artists { get; }
        public ImmutableArray<char> SearchLetters { get; }
        public PlayerState PlayerState { get; }

        public AppState()
        {
            PlayerState = new PlayerState(50);
            SearchLetters = ImmutableArray<char>.Empty;
            Artists = ImmutableArray<ArtistModel>.Empty;
            State = AppStateEnum.Explorer;
            MenuIndex = MenuStateEnum.Home;
            Artist = null;
            Album = null;
        }

        public AppState(AppStateEnum state, MenuStateEnum menuIndex, ArtistModel artist, AlbumModel album, ImmutableArray<ArtistModel> artists, ImmutableArray<char> searchLetters, PlayerState playerState)
        {
            State = state;
            MenuIndex = menuIndex;
            Artist = artist;
            Album = album;
            Artists = artists;
            SearchLetters = searchLetters;
            PlayerState = playerState;
        }

        public struct Builder
        {
            private readonly AppState _state;

            public PlayerState PlayerState;
            public MenuStateEnum MenuIndex;
            public AppStateEnum State;
            public ArtistModel Artist;
            public AlbumModel Album;
            public ImmutableArray<ArtistModel> Artists;
            public ImmutableArray<char> SearchLetters;

            public Builder(AppState state)
            {
                _state = state;

                State = state.State;
                MenuIndex = state.MenuIndex;
                Artist = state.Artist;
                Album = state.Album;
                Artists = state.Artists;
                SearchLetters = state.SearchLetters;
                PlayerState = state.PlayerState;
            }

            public bool Equals(AppState other)
            {
                return State == other.State &&
                    MenuIndex == other.MenuIndex &&
                    ReferenceEquals(Artist, other.Artist) &&
                    ReferenceEquals(Album, other.Album) &&
                    ReferenceEquals(PlayerState, other.PlayerState) &&
                    Artists == other.Artists &&
                    SearchLetters == other.SearchLetters;
            }

            public AppState Build()
            {
                if (Equals(_state)) return _state;

                return new AppState(State, MenuIndex, Artist, Album, Artists, SearchLetters, PlayerState);
            }
        }

    }
}
