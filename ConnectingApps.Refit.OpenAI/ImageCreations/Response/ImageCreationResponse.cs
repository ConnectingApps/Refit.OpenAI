namespace ConnectingApps.Refit.OpenAI.ImageCreations.Response
{
    public class ImageCreationResponse
    {
        public int Created { get; set; }
        public Datum[] Data { get; set; } = null!;
    }
}
