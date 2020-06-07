namespace BeefWebClient.Models
{
    public class ActiveItem
    {
        public string PlaylistId { get; set; }
        public int PlaylistIndex { get; set; }
        public int Index { get; set; }
        public double Position { get; set; }
        public double Duration { get; set; }
        public string[] Columns { get; set; }
    }
}
