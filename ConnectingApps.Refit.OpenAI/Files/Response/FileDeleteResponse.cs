namespace ConnectingApps.Refit.OpenAI.Files.Response
{
    public class FileDeleteResponse
    {
        public string Object { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool Deleted { get; set; }
    }
}
