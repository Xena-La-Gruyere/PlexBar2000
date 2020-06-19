using Library.Abstractions.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class PlayTrackAndAlbumAction : IAction
    {
        public AlbumModel Album { get; }
        public TrackModel Track { get; }

        public PlayTrackAndAlbumAction(AlbumModel album, TrackModel track)
        {
            Album = album;
            Track = track;
        }
    }
}
