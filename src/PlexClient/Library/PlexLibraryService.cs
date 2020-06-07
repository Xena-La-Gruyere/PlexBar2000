using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PlexClient.Client;
using PlexClient.Client.Models;
using PlexClient.Library.Models;

namespace PlexClient.Library
{
    public class PlexLibraryService : IPlexLibraryService
    {
        private readonly IPlexService _plexService;

        public IObservable<ArtistModel[]> Artists { get; }
        private readonly ISubject<ArtistModel[]> _artists;
        public IObservable<char[]> SearchLetters { get; }
        private readonly ISubject<char[]> _searchLetters;
        private Directory _section;
        public PlexLibraryService(IPlexService plexService)
        {
            _plexService = plexService;
            _artists = new ReplaySubject<ArtistModel[]>(1);
            _searchLetters = new ReplaySubject<char[]>(1);

            SearchLetters = _searchLetters;
            Artists = _artists;
        }

        public async Task Initialize()
        {
            var sections = await _plexService.GetSections();

            _section = sections.MediaContainer.Directory.FirstOrDefault(d => d.Type == "artist");

            await RefreshArtists();
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
            => new ArtistModel
            {
                Key = artist.Key,
                Title = artist.Title,
                Guid = artist.Guid,
                Index = artist.Index,
                LetterSearch = FirstLetter(artist),
                RatingKey = artist.RatingKey,
                ThumbnailUrl = _plexService.GetThumbnailUri(artist.Thumb)
            };

        public async Task RefreshArtists()
        {
            if (_section is null) return;

            var artists = await _plexService.GetAllArtists(_section.Key);

            var artistsModels = artists.MediaContainer.Metadata.Select(ToArtistModel)
                .OrderBy(c => c.LetterSearch)
                .ToArray();
            _artists.OnNext(artistsModels);

            _searchLetters.OnNext(
                artistsModels
                    .Select(a => a.LetterSearch)
                    .Where(c => !char.IsSymbol(c) || c == '#')
                    .Where(c => !char.IsPunctuation(c))
                    .OrderBy(c => c)
                    .Distinct()
                    .ToArray());
        }
    }
}
