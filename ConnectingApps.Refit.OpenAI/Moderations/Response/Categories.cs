using System.Text.Json.Serialization;

namespace ConnectingApps.Refit.OpenAI.Moderations.Response
{
    public class Categories
    {
        public bool Sexual { get; set; }
        public bool Hate { get; set; }
        public bool Harassment { get; set; }
        public bool SelfHarm { get; set; }
        [JsonPropertyName("sexual/minors")]
        public bool SexualMinors { get; set; }
        [JsonPropertyName("hate/threatening")]
        public bool HateThreatening { get; set; }
        [JsonPropertyName("violence/graphic")]
        public bool ViolenceGraphic { get; set; }
        [JsonPropertyName("self-harm/intent")]
        public bool SelfHarmIntent { get; set; }
        [JsonPropertyName("self-harm/instructions")]
        public bool SelfHarmInstructions { get; set; }
        [JsonPropertyName("harassment/threatening")]
        public bool HarassmentThreatening { get; set; }
        public bool Violence { get; set; }
    }
}
