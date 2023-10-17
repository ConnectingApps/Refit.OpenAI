namespace ConnectingApps.Refit.OpenAI.Embeddings.Response
{
    public class EmbeddingResponse
    {
        public string Object { get; set; } = null!;
        public Datum[] Data { get; set; } = null!;
        public string Model { get; set; } = null!;
        public Usage Usage { get; set; } = null!;
    }
}
