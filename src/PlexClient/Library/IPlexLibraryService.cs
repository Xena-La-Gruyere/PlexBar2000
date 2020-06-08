using System;
using System.Threading.Tasks;
using PlexClient.Library.Models;

namespace PlexClient.Library
{
    public interface IPlexLibraryService
    {
        IObservable<ArtistModel[]> Artists { get; }
        IObservable<char[]> SearchLetters { get; }

        Task Initialize();
        Task RefreshArtists();
        Task<ArtistModel> GetArtist(ArtistModel artistModel);
        Task<AlbumModel> GetAlbum(AlbumModel albumModel);
    }
}
