﻿using System;

namespace Library.Abstractions.Models
{
    public class TrackModel
    {
        public TrackModel(string title, long index, long duration, string codec, long bitrate, string key, string artist, string album, Uri thumbnailUrl, Uri resource, TrackState trackState)
        {
            Title = title;
            Index = index;
            Duration = duration;
            Codec = codec;
            Bitrate = bitrate;
            Key = key;
            Artist = artist;
            Album = album;
            ThumbnailUrl = thumbnailUrl;
            Resource = resource;
            TrackState = trackState;
        }

        public string Title { get; }
        public string Artist { get; }
        public string Album { get; }
        public long Index { get; }
        public long Duration { get; }
        public string Codec { get; }
        public long Bitrate { get; }
        public string Key { get; }
        public Uri ThumbnailUrl { get; }
        public Uri Resource { get; }
        public TrackState TrackState { get; }

        public struct Builder
        {
            private readonly TrackModel _track;

            public string Title;
            public long Index;
            public long Duration;
            public string Codec;
            public long Bitrate;
            public string Key;
            public string Artist;
            public string Album;
            public Uri ThumbnailUrl;
            public Uri Resource;
            public TrackState TrackState;

            public Builder(TrackModel track)
            {
                _track = track;

                Title = track.Title;
                Index = track.Index;
                Duration = track.Duration;
                Codec = track.Codec;
                Bitrate = track.Bitrate;
                Key = track.Key;
                Artist = track.Artist;
                Album = track.Album;
                ThumbnailUrl = track.ThumbnailUrl;
                Resource = track.Resource;
                TrackState = track.TrackState;
            }

            bool Equal(TrackModel other)
            {
                return Title == other.Title &&
                       Index == other.Index &&
                       Duration == other.Duration &&
                       Codec == other.Codec &&
                       Bitrate == other.Bitrate &&
                       Artist == other.Artist &&
                       Album == other.Album &&
                       ThumbnailUrl == other.ThumbnailUrl &&
                       Resource == other.Resource &&
                       TrackState == other.TrackState &&
                       Key == other.Key;
            }

            public TrackModel Build()
            {
                if (Equal(_track)) return _track;

                return new TrackModel(Title, Index, Duration, Codec, Bitrate, Key, Artist, Album, ThumbnailUrl, Resource, TrackState);
            }
        }
    }
}
