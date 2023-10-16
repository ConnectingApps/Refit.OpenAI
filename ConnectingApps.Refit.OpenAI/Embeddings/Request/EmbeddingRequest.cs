namespace ConnectingApps.Refit.OpenAI.Embeddings.Request
{
    public class EmbeddingRequest
    {
        public string Input { get; set; } = null!;
        public string Model { get; set; } = null!;
    }
}
