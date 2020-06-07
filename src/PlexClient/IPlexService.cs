using System.Threading.Tasks;
using PlexClient.Models;

namespace PlexClient
{
    public interface IPlexService
    {
        Task<Sections> GetSections();

        Task<AllArtists> GetAllArtists(string sectionKey);
    }
}
