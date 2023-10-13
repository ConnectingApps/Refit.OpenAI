using FluentAssertions;
using Refit;
using ConnectingApps.Refit.OpenAI.Moderations;
using ConnectingApps.Refit.OpenAI.Moderations.Request;
using ConnectingApps.Refit.OpenAI.Moderations.Response;
using System.Net;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class ModerationTest
    {
        private static readonly Func<ModerationRequest, Task<ApiResponse<ModerationsResponse>>> CompletionCaller;

        static ModerationTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            var completionApi = RestService.For<IModeration>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
            CompletionCaller = request => completionApi.CreateModerationAsync(request, $"Bearer {apiKey}");
        }


        [Fact]
        public async Task ModerateThis()
        {
            var response = await CompletionCaller(new ModerationRequest
            {
                Input = "I want to hit my dog."
            });
            (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
            response.Content!.Results[0].Categories.Violence.Should().BeTrue();
        }
    }
}
