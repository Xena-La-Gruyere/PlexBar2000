using System;
using System.Collections.Immutable;
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
