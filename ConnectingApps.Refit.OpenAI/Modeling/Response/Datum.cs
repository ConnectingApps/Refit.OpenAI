namespace ConnectingApps.Refit.OpenAI.Modeling.Response
{
    public class Datum
    {
        public string Id { get; set; } = null!;
        public string Object { get; set; } = null!;
        public long Created { get; set; }
        public string OwnedBy { get; set; } = null!;
    }
}