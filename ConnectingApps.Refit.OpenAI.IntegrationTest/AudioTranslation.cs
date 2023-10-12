using FluentAssertions;
using Refit;
using System.Net;
using ConnectingApps.Refit.OpenAI.AudioTranslation;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class AudioTranslationTest
    {
        private readonly string _authorizationHeader;
        private readonly IAudioTranslation _translationApi;

        public AudioTranslationTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            _authorizationHeader = $"Bearer {apiKey}";
            _translationApi = RestService.For<IAudioTranslation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
        }

        [Fact]
        public async Task CallAudioTranslation()
        {
            await using (var recording = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
            {
                var streamPart = new StreamPart(recording, "HalloWereld.mp3");
                var response = await _translationApi.GetAudioTranslation(_authorizationHeader, streamPart, "whisper-1");
                (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
                response.Content!.Text.Should().Contain("Hello world");
            }
        }
    }
}
