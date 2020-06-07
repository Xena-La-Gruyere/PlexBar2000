namespace BeefWebClient.Models
{
    public class Volume
    {
        public VolumeType Type { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Value { get; set; }
        public bool IsMuted { get; set; }
    }
}
