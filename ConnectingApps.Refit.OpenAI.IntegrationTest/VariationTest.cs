using ConnectingApps.Refit.OpenAI.Variations;
using FluentAssertions;
using Refit;
using System.Net;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class VariationTest
    {
        private readonly string _authorizationHeader;
        private readonly IVariation _variationApi;

        public VariationTest() 
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            _authorizationHeader = $"Bearer {apiKey}";
            _variationApi = RestService.For<IVariation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
        }

        [Fact]
        public async Task CallVariation()
        {
            await using (var image = new FileStream("otter.png", FileMode.Open, FileAccess.Read))
            {
                var streamPart = new StreamPart(image, "otter.png");
                var response = await _variationApi.GetImageVariations(_authorizationHeader, streamPart, 2, "1024x1024");
                (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
                response.Content!.Data.Should().HaveCount(2);
            }
        }
    }
}
