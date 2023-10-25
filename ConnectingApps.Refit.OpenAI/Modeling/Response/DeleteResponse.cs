namespace ConnectingApps.Refit.OpenAI.Modeling.Response
{
    public class DeleteResponse
    {
        public string Object { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool Deleted { get; set; }
    }
}
