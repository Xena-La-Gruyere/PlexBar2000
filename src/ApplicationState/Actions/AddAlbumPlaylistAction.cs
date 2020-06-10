using PlexClient.Library.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class AddAlbumPlaylistAction : IAction
    {
        public AddAlbumPlaylistAction(AlbumModel album)
        {
            Album = album;
        }

        public AlbumModel Album { get; }
    }
}
