using PlexClient.Client.Models;
using PlexClient.Library.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class SelectArtist : IAction
    {
        public SelectArtist(ArtistModel artist)
        {
            Artist = artist;
        }

        public ArtistModel Artist { get; private set; }
    }
}
