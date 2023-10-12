using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.AudioTranslation;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var image = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IAudioTranslation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(image, "HalloWereld.mp3");
    var response = await openAiApi.GetAudioTranslation(authorizationHeader, streamPart, "whisper-1");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Translated text {response.Content!.Text}");
}