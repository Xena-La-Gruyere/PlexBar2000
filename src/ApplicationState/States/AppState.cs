using System.Collections.Immutable;
using ApplicationState.Enumerations;
using PlexClient.Library.Models;

namespace ApplicationState.States
{
    public class AppState
    {
        public AppStateEnum State { get; }
        public MenuStateEnum MenuIndex { get; }
        public ArtistModel Artist { get; }
        public AlbumModel Album { get; }
        public ImmutableArray<ArtistModel> Artists { get; }
        public ImmutableArray<AlbumModel> Playlist { get; }
        public ImmutableArray<char> SearchLetters { get; }

        public AppState()
        {
            SearchLetters = ImmutableArray<char>.Empty; ;
            Playlist = ImmutableArray<AlbumModel>.Empty;
            Artists = ImmutableArray<ArtistModel>.Empty;
            State = AppStateEnum.Explorer;
            MenuIndex = MenuStateEnum.Home;
            Artist = null;
            Album = null;
        }

        public AppState(AppStateEnum state, MenuStateEnum menuIndex, ArtistModel artist, AlbumModel album, ImmutableArray<ArtistModel> artists, ImmutableArray<AlbumModel> playlist, ImmutableArray<char> searchLetters)
        {
            State = state;
            MenuIndex = menuIndex;
            Artist = artist;
            Album = album;
            Artists = artists;
            Playlist = playlist;
            SearchLetters = searchLetters;
        }

        public struct Builder
        {
            private readonly AppState _state;

            public MenuStateEnum MenuIndex;
            public AppStateEnum State;
            public ArtistModel Artist;
            public AlbumModel Album;
            public ImmutableArray<ArtistModel> Artists;
            public ImmutableArray<AlbumModel> Playlist;
            public ImmutableArray<char> SearchLetters;

            public Builder(AppState state)
            {
                _state = state;

                State = state.State;
                MenuIndex = state.MenuIndex;
                Artist = state.Artist;
                Album = state.Album;
                Artists = state.Artists;
                Playlist = state.Playlist;
                SearchLetters = state.SearchLetters;
            }

            public bool Equals(AppState other)
            {
                return State == other.State &&
                    MenuIndex == other.MenuIndex &&
                    ReferenceEquals(Artist, other.Artist) &&
                    ReferenceEquals(Album, other.Album) &&
                    Artists == other.Artists &&
                    SearchLetters == other.SearchLetters &&
                    Playlist == other.Playlist;
            }

            public AppState Build()
            {
                if (Equals(_state)) return _state;

                return new AppState(State, MenuIndex, Artist, Album, Artists, Playlist, SearchLetters);
            }
        }

    }
}
