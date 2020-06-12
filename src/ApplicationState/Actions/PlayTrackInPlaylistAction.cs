using PlexClient.Library.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class PlayTrackInPlaylistAction : IAction
    {
        public TrackModel Track { get; }

        public PlayTrackInPlaylistAction(TrackModel track)
        {
            Track = track;
        }
    }
}
