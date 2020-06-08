using ApplicationState.Enumerations;
using PlexClient.Library.Models;

namespace ApplicationState.States
{
    public class AppState
    {
        public AppStateEnum State { get; }
        public int MenuIndex { get; }
        public ArtistModel Artist { get; }
        public AlbumModel Album { get; }

        public AppState()
        {
            State = AppStateEnum.Explorer;
            MenuIndex = 0;
            Artist = null;
            Album = null;
        }

        public AppState(AppStateEnum state, int menuIndex, ArtistModel artist, AlbumModel album)
        {
            State = state;
            MenuIndex = menuIndex;
            Artist = artist;
            Album = album;
        }

        public struct Builder
        {
            private readonly AppState _state;

            public int MenuIndex;
            public AppStateEnum State;
            public ArtistModel Artist;
            public AlbumModel Album;

            public Builder(AppState state)
            {
                _state = state;

                State = state.State;
                MenuIndex = state.MenuIndex;
                Artist = state.Artist;
                Album = state.Album;
            }

            public bool Equals(AppState other)
            {
                return State == other.State &&
                    MenuIndex == other.MenuIndex &&
                    ReferenceEquals(Artist, other.Artist) &&
                    ReferenceEquals(Album, other.Album);
            }

            public AppState Build()
            {
                if (Equals(_state)) return _state;

                return new AppState(State, MenuIndex, Artist, Album);
            }
        }

    }
}
