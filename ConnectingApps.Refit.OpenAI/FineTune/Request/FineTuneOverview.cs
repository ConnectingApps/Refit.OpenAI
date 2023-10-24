namespace ConnectingApps.Refit.OpenAI.FineTune.Request
{
    public class FineTuneOverview
    {
        public string Object { get; set; } = null!;

        public FineTuneOverview[] Data { get; set; } = null!;

        public bool HasMore { get; set; }
    }
}
