using System.Collections.Immutable;
using PlexClient.Library.Models;
using Redux;

namespace ApplicationState.Actions
{
    public class ArtistsLoaded : IAction
    {

        public ArtistsLoaded(ImmutableArray<ArtistModel> artist)
        {
            Artist = artist;
        }

        public ImmutableArray<ArtistModel> Artist { get; }
    }
}
