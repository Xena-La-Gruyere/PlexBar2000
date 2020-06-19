using System.Threading.Tasks;
using Library.Abstractions.Models;

namespace Library.Abstractions
{
    public interface ILibraryService
    {
        Task<ArtistModel[]> GetArtists();
        Task<ArtistModel> GetArtist(ArtistModel artistModel);
        Task<AlbumModel> GetAlbum(AlbumModel albumModel);
    }
}
