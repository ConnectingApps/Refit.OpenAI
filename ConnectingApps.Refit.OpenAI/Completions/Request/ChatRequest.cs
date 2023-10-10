using System.Collections.Generic;

namespace ConnectingApps.Refit.OpenAI.Completions.Request
{
    public class ChatRequest
    {
        public string Model { get; set; } = null!;
        public List<Message> Messages { get; set; } = new List<Message>();
        public double? FrequencyPenalty { get; set; } = 0;
        public string? FunctionCall { get; set; } = null!;
        public List<string>? Functions { get; set; } = null; // Adjust if you have specific function objects
        public Dictionary<string, double>? LogitBias { get; set; } = null;
        public int? MaxTokens { get; set; } = null; // or set a default value if necessary
        public int? N { get; set; } = 1; // Default value assuming you usually want one completion
        public double? PresencePenalty { get; set; } = 0;
        public List<string>? Stop { get; set; } = null;
        public bool? Stream { get; set; } = false;
        public double? Temperature { get; set; } = 1; // Default value is assumed to be 1
        public double? TopP { get; set; } = 1; // Default value is assumed to be 1
        public string? User { get; set; } = null;
    }
}
