using PlexClient.Client.Models;

namespace PlexClient.Library.Models
{
    public class TrackModel
    {
        public TrackModel(string title, long index, long duration, string codec, long bitrate, string key)
        {
            Title = title;
            Index = index;
            Duration = duration;
            Codec = codec;
            Bitrate = bitrate;
            Key = key;
        }

        public TrackModel(Track track)
        {
            Title = track.Title;
            Index = track.Index;
            Duration = track.Duration;
            Codec = track.Media[0].AudioCodec;
            Bitrate = track.Media[0].Bitrate;
            Key = track.Media[0].Part[0].Key;
        }

        public string Title { get; }
        public long Index { get; }
        public long Duration { get; }
        public string Codec { get; }
        public long Bitrate { get; }
        public string Key { get; }

        public struct Builder
        {
            private readonly TrackModel _track;

            public string Title;
            public long Index;
            public long Duration;
            public string Codec;
            public long Bitrate;
            public string Key;

            public Builder(TrackModel track)
            {
                _track = track;

                Title = track.Title;
                Index = track.Index;
                Duration = track.Duration;
                Codec = track.Codec;
                Bitrate = track.Bitrate;
                Key = track.Key;
            }

            bool Equal(TrackModel other)
            {
                return Title == other.Title &&
                       Index == other.Index &&
                       Duration == other.Duration &&
                       Codec == other.Codec &&
                       Bitrate == other.Bitrate &&
                       Key == other.Key;
            }

            public TrackModel Build()
            {
                if (Equal(_track)) return _track;

                return new TrackModel(Title, Index, Duration, Codec, Bitrate, Key);
            }
        }
    }
}
