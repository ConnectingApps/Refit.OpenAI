namespace ConnectingApps.Refit.OpenAI.Files.Response
{
    public class FilePostResponse
    {
        public string Object { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public string Filename { get; set; } = null!;
        public long Bytes { get; set; }
        public long CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public string StatusDetails { get; set; } = null!;
    }
}
