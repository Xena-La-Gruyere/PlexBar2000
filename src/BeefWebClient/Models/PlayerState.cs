namespace BeefWebClient.Models
{
    public class PlayerState
    {
        public PlayerInfo Info { get; set; }
        public ActiveItem ActiveItem { get; set; }
        public PlaybackState PlaybackState { get; set; }
        public int PlaybackMode { get; set; }
        public string[] PlaybackModes { get; set; }
        public Volume Volume { get; set; }

    }
}
