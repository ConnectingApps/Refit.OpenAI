namespace ConnectingApps.Refit.OpenAI.ImageCreations.Request
{
    public class ImageCreationRequest
    {
        public string Prompt { get; set; } = null!;
        public int N { get; set; }
        public string Size { get; set; } = null!;
    }
}
