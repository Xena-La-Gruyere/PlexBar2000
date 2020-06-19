using System;
using System.Collections.Immutable;

namespace Library.Abstractions.Models
{
    public class ArtistModel
    {
        public ArtistModel(string key, string title, Uri thumbnailUrl, char letterSearch, string bio, ImmutableArray<AlbumModel> albums)
        {
            Key = key;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            LetterSearch = letterSearch;
            Bio = bio;
            Albums = albums;
        }

        public string Key { get; }
        public string Title { get; }
        public Uri ThumbnailUrl { get; }
        public char LetterSearch { get; }
        public string Bio { get; }
        public ImmutableArray<AlbumModel> Albums { get; }

        public struct Builder
        {
            public string Key;
            public string Title;
            public Uri ThumbnailUrl;
            public char LetterSearch;
            public string Bio;
            public ImmutableArray<AlbumModel> Albums;

            public Builder(ArtistModel state)
            {
                Key = state.Key;
                Title = state.Title;
                ThumbnailUrl = state.ThumbnailUrl;
                LetterSearch = state.LetterSearch;
                Bio = state.Bio;
            }

            public ArtistModel Build()
                => new ArtistModel(Key, Title, ThumbnailUrl, LetterSearch, Bio, Albums);
        }
    }
}
