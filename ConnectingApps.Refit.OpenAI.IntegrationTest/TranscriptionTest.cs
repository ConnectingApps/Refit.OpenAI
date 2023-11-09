using ConnectingApps.Refit.OpenAI.Transcriptions;
using FluentAssertions;
using Refit;
using System.Net;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest;

public class TranscriptionTest
{
    private readonly string _authorizationHeader;
    private readonly ITranscription _transcriptionApi;

    public TranscriptionTest()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
        apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
        _authorizationHeader = $"Bearer {apiKey}";
        _transcriptionApi = RestService.For<ITranscription>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    }

    [Fact]
    public async Task CallGetAudioTranscription()
    {
        await using (var recording = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
        {
            var streamPart = new StreamPart(recording, "HalloWereld.mp3");
            var response = await _transcriptionApi.GetAudioTranscription(_authorizationHeader, streamPart, "whisper-1");
            (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
            response.Content!.Text.Should().Contain("Hallo wereld");
        }
    }
}