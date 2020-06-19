using Library.Abstractions.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class RemoveAlbumPlaylistAction : IAction
    {
        public RemoveAlbumPlaylistAction(AlbumModel album)
        {
            Album = album;
        }

        public AlbumModel Album { get; }
    }
}
