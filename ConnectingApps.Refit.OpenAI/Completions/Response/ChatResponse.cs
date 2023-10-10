namespace ConnectingApps.Refit.OpenAI.Completions.Response
{
    public class ChatResponse
    {
        public string Id { get; set; } = null!;
        public string Object { get; set; } = null!;
        public long Created { get; set; }
        public string Model { get; set; } = null!;
        public Choice[] Choices { get; set; } = null!;
        public Usage Usage { get; set; } = null!;
    }
}
