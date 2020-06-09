using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlexClient.Library.Models;

namespace Playlist
{
    public class PlaylistService : IPlaylistService
    {
        public IObservable<ImmutableArray<TrackModel>> Tracks { get; set; }
        private readonly BehaviorSubject<ImmutableArray<TrackModel>> _tracks;

        public PlaylistService()
        {
            _tracks = new BehaviorSubject<ImmutableArray<TrackModel>>(ImmutableArray<TrackModel>.Empty);
            Tracks = _tracks.DistinctUntilChanged();
        }

        public void AddTracks(params TrackModel[] tracks)
        {
            _tracks.OnNext(_tracks.Value.AddRange(tracks));
        }

        public void RemoveTracks(params TrackModel[] tracks)
        {
            _tracks.OnNext(_tracks.Value.RemoveRange(tracks));
        }

        public void Clear()
        {
            _tracks.OnNext(_tracks.Value.RemoveRange(ImmutableArray<TrackModel>.Empty));
        }
    }
}
