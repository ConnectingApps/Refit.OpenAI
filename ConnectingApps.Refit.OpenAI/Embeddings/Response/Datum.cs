namespace ConnectingApps.Refit.OpenAI.Embeddings.Response
{
    public class Datum
    {
        public string Object { get; set; } = null!;
        public int Index { get; set; }
        public double[] Embedding { get; set; } = null!;
    }
}