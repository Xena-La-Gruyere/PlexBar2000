using System.Text.Json.Serialization;

namespace PlexClient.Models
{
    public partial class Sections
    {
        [JsonPropertyName("MediaContainer")]
        public SectionsContainer MediaContainer { get; set; }
    }

    public partial class SectionsContainer
    {
        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("allowSync")]
        public bool AllowSync { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("mediaTagPrefix")]
        public string MediaTagPrefix { get; set; }

        [JsonPropertyName("mediaTagVersion")]
        public long MediaTagVersion { get; set; }

        [JsonPropertyName("title1")]
        public string Title1 { get; set; }

        [JsonPropertyName("Directory")]
        public Directory[] Directory { get; set; }
    }
}
