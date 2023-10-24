namespace ConnectingApps.Refit.OpenAI.FineTune.Request
{
    public class FineTuneRequest
    {
        public string TrainingFile { get; set; } = null!;
        public string Model { get; set; } = null!;
    }
}
