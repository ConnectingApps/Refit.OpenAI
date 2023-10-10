using System.Net;
using ConnectingApps.Refit.OpenAI.Completions;
using ConnectingApps.Refit.OpenAI.Completions.Request;
using FluentAssertions;
using Refit;
using ConnectingApps.Refit.OpenAI.Completions.Response;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class CompletionTest
    {
        private static readonly Func<ChatRequest, Task<ApiResponse<ChatResponse>>> CompletionCaller;

        static CompletionTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            var completionApi = RestService.For<ICompletion>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
            CompletionCaller = chatRequest => completionApi.CreateCompletionAsync(chatRequest, $"Bearer {apiKey}");
        }

        [Fact]
        public async Task CapitalOfFrance()
        {
            var response = await CompletionCaller(new ChatRequest
            {
                Model = "gpt-3.5-turbo",
                Temperature = 0.7,
                Messages = new List<Message>
                {
                    new()
                    {
                        Role = "user",
                        Content = "What is the capital of the France?",
                    }
                }
            });
            (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
            response.Content!.Choices.First().Message.Content.Should().Contain("Paris");
        }
    }
}