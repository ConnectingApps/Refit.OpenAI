using Refit;
using System.Text.Json;

namespace ConnectingApps.Refit.OpenAI
{
    public class OpenAiRefitSettings : RefitSettings
    {
        public static OpenAiRefitSettings RefitSettings { get; } = new OpenAiRefitSettings();

        public OpenAiRefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy(),
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}
