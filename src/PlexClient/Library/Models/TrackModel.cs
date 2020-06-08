using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Client.Models;

namespace PlexClient.Library.Models
{
    public class TrackModel
    {
        public string Title { get; }
        public long Index { get; }
        public long Duration { get; }
        public string Codec { get; }
        public long Bitrate { get; }
        public string Key { get; }
        public TrackModel(Track track)
        {
            Title = track.Title;
            Index = track.Index;
            Duration = track.Duration;
            Codec = track.Media[0].AudioCodec;
            Bitrate = track.Media[0].Bitrate;
            Key = track.Media[0].Part[0].Key;
        }
    }
}
