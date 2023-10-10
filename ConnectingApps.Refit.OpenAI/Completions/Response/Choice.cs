namespace ConnectingApps.Refit.OpenAI.Completions.Response
{
    public class Choice
    {
        public int? Index { get; set; }
        public Message? Message { get; set; } = null!;
        public string? FinishReason { get; set; } = null!;
    }
}
