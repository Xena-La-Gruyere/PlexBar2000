using System.Globalization;
using System.Text;

namespace Library.Abstractions
{
    public static class Utils
    {
        private static bool IsInRange(char e, int min, int max)
        {
            return e >= min && e <= max;
        }

        private static string RemoveDiacritics(string text)
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

        public static char FirstLetter(string text)
        {
            var letter = RemoveDiacritics(text.ToUpperInvariant())[0];

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
    }
}
