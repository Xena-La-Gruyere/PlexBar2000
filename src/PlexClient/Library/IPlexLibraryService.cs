using System.Threading.Tasks;
using PlexClient.Library.Models;

namespace PlexClient.Library
{
    public interface IPlexLibraryService
    {
        Task<ArtistModel[]> GetArtists();
        Task<ArtistModel> GetArtist(ArtistModel artistModel);
        Task<AlbumModel> GetAlbum(AlbumModel albumModel);
    }
}
