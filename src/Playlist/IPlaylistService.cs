using System;
using System.Collections.Immutable;
using PlexClient.Library.Models;

namespace Playlist
{
    public interface IPlaylistService
    {
        IObservable<ImmutableArray<AlbumModel>> Albums { get; }

        void AddAlbum(AlbumModel album);
        void RemoveAlbum(AlbumModel album);
        void Clear();
    }
}
