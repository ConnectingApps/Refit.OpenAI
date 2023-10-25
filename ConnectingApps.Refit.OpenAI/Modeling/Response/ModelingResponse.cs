namespace ConnectingApps.Refit.OpenAI.Modeling.Response
{
    public class ModelingResponse
    {
        public string Object { get; set; } = null!;
        public Datum[] Data { get; set; } = null!;
    }
}
