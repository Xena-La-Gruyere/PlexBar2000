using Library.Abstractions.Models;

namespace ApplicationState.Actions
{
    public class ArtistLoaded
    {
        public ArtistModel ArtistOld { get; }
        public ArtistModel ArtistNew { get; }

        public ArtistLoaded(ArtistModel artistOld, ArtistModel artistNew)
        {
            ArtistOld = artistOld;
            ArtistNew = artistNew;
        }
    }
}
