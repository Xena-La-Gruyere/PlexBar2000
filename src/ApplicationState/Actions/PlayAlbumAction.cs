using Library.Abstractions.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class PlayAlbumAction : IAction
    {

        public PlayAlbumAction(AlbumModel album)
        {
            Album = album;
        }

        public AlbumModel Album { get; }
    }
}
