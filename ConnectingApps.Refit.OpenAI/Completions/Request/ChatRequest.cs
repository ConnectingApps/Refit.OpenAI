using System.Collections.Generic;

namespace ConnectingApps.Refit.OpenAI.Completions.Request
{
    public class ChatRequest
    {
        public string Model { get; set; } = null!;
        public List<Message> Messages { get; set; } = new List<Message>();
        public double? FrequencyPenalty { get; set; }
        public string? FunctionCall { get; set; }
        public List<string>? Functions { get; set; } // Adjust if you have specific function objects
        public Dictionary<string, double>? LogitBias { get; set; }
        public int? MaxTokens { get; set; } // or set a default value if necessary
        public int? N { get; set; } // Default value assuming you usually want one completion
        public double? PresencePenalty { get; set; }
        public List<string>? Stop { get; set; }
        public bool? Stream { get; set; }
        public double? Temperature { get; set; } // Default value is assumed to be 1
        public double? TopP { get; set; } // Default value is assumed to be 1
        public string? User { get; set; }
    }
}
