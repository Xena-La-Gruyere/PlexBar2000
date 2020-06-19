using System;
using System.Collections.Immutable;
using Library.Abstractions.Models;
using PlexClient.Client.Models;

namespace PlexClient.Library
{
    public static class Extensions
    {
        public static AlbumModel ToModel(this Album album, Uri thumbnailUrl,string artist)
            => new AlbumModel(album.RatingKey,
                album.Title,
                thumbnailUrl,
                album.Year,
                ImmutableArray<TrackModel>.Empty,
                artist);

        public static ArtistModel ToModel(this Artist artist, Uri thumbnailUrl, char letterSearch)
            => new ArtistModel(artist.RatingKey,
                artist.Title,
                thumbnailUrl,
                letterSearch,
                artist.Summary,
                ImmutableArray<AlbumModel>.Empty);


        public static TrackModel ToModel(this Track track, AlbumModel albumModel, Uri resource)
            => new TrackModel(
                track.Title,
                track.Index,
                track.Duration,
                track.Media[0].AudioCodec,
                track.Media[0].Bitrate,
                track.Media[0].Part[0].Key,
                albumModel.Artist,
                albumModel.Title,
                albumModel.ThumbnailUrl,
                resource,
                TrackState.Nothing
                );
    }
}
