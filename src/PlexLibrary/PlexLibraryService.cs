using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient;
using PlexClient.Models;

namespace PlexLibrary
{
    public class PlexLibraryService : IPlexLibraryService
    {
        public PlexLibraryService(IPlexService plexService)
        {
            
        }

        public IObservable<Artist[]> Artists { get; }
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
