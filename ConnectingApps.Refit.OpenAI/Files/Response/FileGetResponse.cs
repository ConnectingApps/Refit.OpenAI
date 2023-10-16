namespace ConnectingApps.Refit.OpenAI.Files.Response
{
    public class FileGetResponse
    {
        public string Object { get; set; } = null!;
        public Datum[] Data { get; set; } = null!;
    }
}
