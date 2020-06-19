using System;
using System.Text.Json.Serialization;

namespace PlexClient.Client.Models
{
    public partial class Album
    {
        [JsonPropertyName("ratingKey")]
        public string RatingKey { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("parentRatingKey")]
        public string ParentRatingKey { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; }

        [JsonPropertyName("parentGuid")]
        public string ParentGuid { get; set; }

        [JsonPropertyName("studio")]
        public string Studio { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("parentKey")]
        public string ParentKey { get; set; }

        [JsonPropertyName("parentTitle")]
        public string ParentTitle { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("index")]
        public long Index { get; set; }

        [JsonPropertyName("year")]
        public long Year { get; set; }

        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }

        [JsonPropertyName("parentThumb")]
        public string ParentThumb { get; set; }

        [JsonPropertyName("originallyAvailableAt")]
        public DateTimeOffset OriginallyAvailableAt { get; set; }

        [JsonPropertyName("addedAt")]
        public long AddedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonPropertyName("loudnessAnalysisVersion")]
        public string LoudnessAnalysisVersion { get; set; }

        [JsonPropertyName("Director")]
        public Director[] Director { get; set; }
    }

    public partial class Director
    {
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
