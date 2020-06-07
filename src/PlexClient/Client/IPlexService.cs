using System.Threading.Tasks;
using PlexClient.Client.Models;

namespace PlexClient.Client
{
    public interface IPlexService
    {
        Task<Sections> GetSections();

        Task<AllArtists> GetAllArtists(string sectionKey);
    }
}
