using FluentAssertions;
using Refit;
using System.Net;
using ConnectingApps.Refit.OpenAI.ImageCreations;
using ConnectingApps.Refit.OpenAI.ImageCreations.Request;
using ConnectingApps.Refit.OpenAI.ImageCreations.Response;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class ImageCreationTest
    {
        private static readonly Func<ImageCreationRequest, Task<ApiResponse<ImageCreationResponse>>> ImageCreationCaller;

        static ImageCreationTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            var completionApi = RestService.For<IImageCreation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
            ImageCreationCaller = chatRequest => completionApi.CreateImageAsync(chatRequest, $"Bearer {apiKey}");
        }

        [Fact]
        public async Task ImageCreationVerify()
        {
            var response = await ImageCreationCaller(new ImageCreationRequest
            {
                N = 2,
                Prompt = "A cute baby sea otter.",
                Size = "1024x1024",
            });
            (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
            response.Content!.Data.Length.Should().Be(2);
            response.Content!.Data.First().Url.Should().NotBeNullOrEmpty();
            response.Content!.Data.Last().Url.Should().NotBeNullOrEmpty();
            response.Content!.Created.Should().BeGreaterThan(0);
        }
    }
}
