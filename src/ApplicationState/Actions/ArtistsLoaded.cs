using System.Collections.Immutable;
using Library.Abstractions.Models;
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
