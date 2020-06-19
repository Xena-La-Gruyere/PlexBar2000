using System;
using System.Collections.Immutable;

namespace Library.Abstractions.Models
{
    public class AlbumModel
    {
        public AlbumModel(string key, string title, Uri thumbnailUrl, long year, ImmutableArray<TrackModel> tracks, string artist)
        {
            Key = key;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            Year = year;
            Tracks = tracks;
            Artist = artist;
        }

        public string Key { get; }
        public string Title { get; }
        public string Artist { get; }
        public Uri ThumbnailUrl { get; }
        public long Year { get; }
        public ImmutableArray<TrackModel> Tracks { get; }

        public struct Builder
        {
            private readonly AlbumModel _state;

            public string Key;
            public string Title;
            public string Artist;
            public Uri ThumbnailUrl;
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
                Artist = state.Artist;
            }

            public bool Equal(AlbumModel other)
            {
                return Key == other.Key &&
                       Title == other.Title &&
                       ThumbnailUrl == other.ThumbnailUrl &&
                       Year == other.Year &&
                       Artist == other.Artist &&
                       Tracks == other.Tracks;
            }

            public AlbumModel Build()
            {
                if (Equal(_state)) return _state;

                return new AlbumModel(Key, Title, ThumbnailUrl, Year, Tracks, Artist);
            }
        }
    }
}
