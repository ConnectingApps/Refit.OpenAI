using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Transcriptions;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var recording = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<ITranscription>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(recording, "HalloWereld.mp3");
    var response = await openAiApi.GetAudioTranscription(authorizationHeader, streamPart, "whisper-1");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Actual text {response.Content!.Text}");
}