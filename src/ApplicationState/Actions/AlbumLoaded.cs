using Library.Abstractions.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class AlbumLoaded : IAction
    {
        public readonly AlbumModel AlbumNew;
        public readonly ArtistModel Artist;
        public readonly AlbumModel Album;

        public AlbumLoaded(ArtistModel artist, AlbumModel album, AlbumModel albumNew)
        {
            AlbumNew = albumNew;
            Artist = artist;
            Album = album;
        }

    }
}
