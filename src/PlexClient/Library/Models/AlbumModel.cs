using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Client.Models;

namespace PlexClient.Library.Models
{
    public class AlbumModel
    {
        public AlbumModel(Album album, string thumbnailUrl)
        {
            Key = album.RatingKey;
            Title = album.Title;
            ThumbnailUrl = thumbnailUrl;
            Year = album.Year;
            Tracks = ImmutableArray<TrackModel>.Empty;
        }

        public AlbumModel(string key, string title, string thumbnailUrl, long year, ImmutableArray<TrackModel> tracks)
        {
            Key = key;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            Year = year;
            Tracks = tracks;
        }

        public string Key { get; }
        public string Title { get; }
        public string ThumbnailUrl { get; }
        public long Year { get; }
        public ImmutableArray<TrackModel> Tracks { get; }

        public struct Builder
        {
            private readonly AlbumModel _state;

            public string Key;
            public string Title;
            public string ThumbnailUrl;
            public long Year;
            public ImmutableArray<TrackModel> Tracks;

            public Builder(AlbumModel state)
            {
                _state = state;

                Key = state.Key;
                Title = state.Title;
                ThumbnailUrl = state.ThumbnailUrl;
                Year = state.Year;
                Tracks = state.Tracks;
            }

            public bool Equal(AlbumModel other)
            {
                return Key == other.Key &&
                       Title == other.Title &&
                       ThumbnailUrl == other.ThumbnailUrl &&
                       Year == other.Year &&
                       Tracks == other.Tracks;
            }

            public AlbumModel Build()
            {
                if (Equal(_state)) return _state;

                return new AlbumModel(Key, Title, ThumbnailUrl, Year, Tracks);
            }
        }
    }
}
