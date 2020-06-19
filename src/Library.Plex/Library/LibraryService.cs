using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Library.Abstractions;
using Library.Abstractions.Models;
using PlexClient.Client;
using PlexClient.Client.Models;

namespace PlexClient.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly IPlexService _plexService;

        private Directory _section;
        public LibraryService(IPlexService plexService)
        {
            _plexService = plexService;
        }

        private ArtistModel ToArtistModel(Artist artist)
            => artist.ToModel(_plexService.GetResourceUri(artist.Thumb), Utils.FirstLetter(artist.Title));

        public async Task<ArtistModel[]> GetArtists()
        {
            var sections = await _plexService.GetSections();

            _section = sections.MediaContainer.Directory.FirstOrDefault(d => d.Type == "artist");

            if (_section is null) return new ArtistModel[0];

            var artists = await _plexService.GetAllArtists(_section.Key);

            return artists.MediaContainer.Metadata.Select(ToArtistModel)
                .OrderBy(c => c.LetterSearch)
                .ToArray();
        }

        public async Task<ArtistModel> GetArtist(ArtistModel artistModel)
        {
            var artist = await _plexService.GetArtist(artistModel.Key);

            var builder = new ArtistModel.Builder(artistModel)
            {
                Albums = artist.MediaContainer.Albums
                    .Select(album => album.ToModel(_plexService.GetResourceUri(album.Thumb), artistModel.Title))
                    .OrderBy(a => a.Year)
                    .ToImmutableArray()
            };

            return builder.Build();
        }

        public async Task<AlbumModel> GetAlbum(AlbumModel albumModel)
        {
            var album = await _plexService.GetAlbum(albumModel.Key);

            var builder = new AlbumModel.Builder(albumModel)
            {
                Tracks = album.MediaContainer.Tracks
                    .OrderBy(t => t.Index)
                    .Select(t => t.ToModel(albumModel, _plexService.GetResourceUri(t.Media[0].Part[0].Key)))
                    .ToImmutableArray()
            };

            return builder.Build();
        }
    }
}
