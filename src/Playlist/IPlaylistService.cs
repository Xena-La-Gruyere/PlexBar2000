using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Library.Models;

namespace Playlist
{
    public interface IPlaylistService
    {
        IObservable<ImmutableArray<TrackModel>> Tracks { get; }

        void AddTracks(params TrackModel[] tracks);
        void RemoveTracks(params TrackModel[] tracks);
        void Clear();
    }
}
