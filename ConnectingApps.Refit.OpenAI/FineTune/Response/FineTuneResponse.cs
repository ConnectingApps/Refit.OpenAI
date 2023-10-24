namespace ConnectingApps.Refit.OpenAI.FineTune.Response
{

    public class FineTuneResponse
    {
        public string Object { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Model { get; set; } = null!;
        public long CreatedAt { get; set; }
        public long? FinishedAt { get; set; }
        public string? FineTunedModel { get; set; }
        public string OrganizationId { get; set; } = null!;
        public string[] ResultFiles { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? ValidationFile { get; set; }
        public string TrainingFile { get; set; } = null!;
        public Hyperparameters Hyperparameters { get; set; } = null!;
        public long? TrainedTokens { get; set; } 
        public object? Error { get; set; } 
    }
}
