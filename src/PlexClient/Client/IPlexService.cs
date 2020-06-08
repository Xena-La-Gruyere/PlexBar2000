using System.Threading.Tasks;
using PlexClient.Client.Models;

namespace PlexClient.Client
{
    public interface IPlexService
    {
        string GetThumbnailUri(string resource);

        Task<Sections> GetSections();

        Task<AllArtists> GetAllArtists(string sectionKey);

        Task<ArtistDetail> GetArtist(string ratingKey);

        Task<AlbumDetail> GetAlbum(string ratingKey);

        Task<byte[]> GetResource(string resource);
    }
}
