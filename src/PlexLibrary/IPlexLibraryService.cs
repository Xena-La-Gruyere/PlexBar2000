using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Models;

namespace PlexLibrary
{
    public interface IPlexLibraryService
    {
        IObservable<Artist[]> Artists { get; }

        void Initialize();
    }
}
