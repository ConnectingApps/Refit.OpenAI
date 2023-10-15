namespace ConnectingApps.Refit.OpenAI.ImageCreations.Response
{
    public class ImageCreationResponse
    {
        public long Created { get; set; }
        public Datum[] Data { get; set; } = null!;
    }
}
