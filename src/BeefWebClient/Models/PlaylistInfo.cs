namespace BeefWebClient.Models
{
    public class PlaylistInfo
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public bool IsCurrent { get; set; }
        public int ItemCount { get; set; }
        public double TotalTime { get; set; }

    }
}
