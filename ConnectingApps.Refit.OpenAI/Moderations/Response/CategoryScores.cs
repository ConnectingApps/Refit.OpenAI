using System.Text.Json.Serialization;

namespace ConnectingApps.Refit.OpenAI.Moderations.Response
{
    public class CategoryScores
    {
        public double Sexual { get; set; }
        public double Hate { get; set; }
        public double Harassment { get; set; }
        [JsonPropertyName("self-harm")]
        public double SelfHarm { get; set; }
        [JsonPropertyName("sexual/minors")]
        public double SexualMinors { get; set; }
        [JsonPropertyName("hate/threatening")]
        public double HateThreatening { get; set; }
        [JsonPropertyName("violence/graphic")]
        public double ViolenceGraphic { get; set; }
        [JsonPropertyName("self-harm/intent")]
        public double SelfHarmIntent { get; set; }
        [JsonPropertyName("self-harm/instructions")]
        public double SelfHarmInstructions { get; set; }
        [JsonPropertyName("harassment/threatening")]
        public double HarassmentThreatening { get; set; }
        public double Violence { get; set; }
    }
}
