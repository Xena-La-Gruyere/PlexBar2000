using System;
using System.Collections.Generic;
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
        }

        public AlbumModel(string key, string title, string thumbnailUrl, int year)
        {
            Key = key;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            Year = year;
        }

        public string Key { get; }
        public string Title { get; }
        public string ThumbnailUrl { get; }
        public long Year { get; }
    }
}
