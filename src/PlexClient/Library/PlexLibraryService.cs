using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Client;
using PlexClient.Client.Models;
using PlexClient.Library.Models;

namespace PlexClient.Library
{
    public class PlexLibraryService : IPlexLibraryService
    {
        private readonly IPlexService _plexService;

        private Directory _section;
        public PlexLibraryService(IPlexService plexService)
        {
            _plexService = plexService;
        }

        private static bool IsInRange(char e, int min, int max)
        {
            return e >= min && e <= max;
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private char FirstLetter(Artist artist)
        {
            var letter = RemoveDiacritics(artist.Title.ToUpperInvariant())[0];

            if (char.IsNumber(letter)) return '#';

            var hiragana = IsInRange(letter, 0x3040, 0x309F);
            var katakana = IsInRange(letter, 0x30A0, 0x30FF);
            var kanji = IsInRange(letter, 0x4E00, 0x9FBF);

            if (hiragana ||
                katakana ||
                kanji)
                return '字';

            return letter;
        }

        private ArtistModel ToArtistModel(Artist artist)
            => new ArtistModel(artist, FirstLetter(artist), _plexService.GetThumbnailUri(artist.Thumb));

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
                    .Select(album => new AlbumModel(album, _plexService.GetThumbnailUri(album.Thumb), artistModel.Title))
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
                    .Select(t => new TrackModel(t, albumModel))
                    .ToImmutableArray()
            };

            return builder.Build();
        }
    }
}
