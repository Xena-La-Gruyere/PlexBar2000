using Library.Abstractions.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class SelectAlbum : IAction
    {
        public SelectAlbum(AlbumModel album)
        {
            Album = album;
        }

        public AlbumModel Album { get; }
    }
}
