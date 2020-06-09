using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlexClient.Library.Models;

namespace Playlist
{
    public class PlaylistService : IPlaylistService
    {
        public IObservable<ImmutableArray<AlbumModel>> Albums { get; set; }
        private readonly BehaviorSubject<ImmutableArray<AlbumModel>> _albums;

        public PlaylistService()
        {
            _albums = new BehaviorSubject<ImmutableArray<AlbumModel>>(ImmutableArray<AlbumModel>.Empty);
            Albums = _albums.DistinctUntilChanged();
        }

        public void AddAlbum(AlbumModel album)
        {
            _albums.OnNext(_albums.Value.Add(album));
        }

        public void RemoveAlbum(AlbumModel album)
        {
            _albums.OnNext(_albums.Value.Add(album));
        }

        public void Clear()
        {
            _albums.OnNext(_albums.Value.RemoveRange(ImmutableArray<AlbumModel>.Empty));
        }
    }
}
