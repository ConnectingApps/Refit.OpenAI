using ConnectingApps.Refit.OpenAI.FineTune.Response;

namespace ConnectingApps.Refit.OpenAI.FineTune.Request
{
    public class FineTuneOverview
    {
        public string Object { get; set; } = null!;

        public FineTuneResponse[] Data { get; set; } = null!;

        public bool HasMore { get; set; }
    }
}
