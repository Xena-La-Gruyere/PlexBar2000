namespace BeefWebClient.Models
{
    public class FileSystemEntry
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileType Type { get; set; }
        public int Size { get; set; }
        public int Timestamp { get; set; }
    }
}
