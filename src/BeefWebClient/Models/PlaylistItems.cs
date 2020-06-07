namespace BeefWebClient.Models
{
    public class PlaylistItems
    {
        public int Offset { get; set; }
        public int TotalCount { get; set; }
        public PlaylistItemInfo[] Items { get; set; }
    }
}