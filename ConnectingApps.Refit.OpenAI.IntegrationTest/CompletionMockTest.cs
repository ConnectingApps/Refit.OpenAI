using ConnectingApps.Refit.OpenAI.Completions;
using ConnectingApps.Refit.OpenAI.Completions.Request;
using ConnectingApps.Refit.OpenAI.Completions.Response;
using Refit;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using WireMock.Server;


namespace ConnectingApps.Refit.OpenAI.IntegrationTest;

public class CompletionMockTest : IDisposable
{
    private readonly Func<ChatRequest, Task<ApiResponse<ChatResponse>>> _completionCaller;
    private readonly WireMockServer _server;

    public CompletionMockTest()
    {
        _server = WireMockServer.Start();
        var completionApi = RestService.For<ICompletion>(_server.Url!, OpenAiRefitSettings.RefitSettings);
        _completionCaller = chatRequest => completionApi.CreateCompletionAsync(chatRequest, $"Bearer DUMMY");
    }

    [Fact]
    public async Task CapitalOfFrance()
    {
        await _completionCaller(new ChatRequest
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

        string expectedRequest = """
                                 {
                                     "model": "gpt-3.5-turbo",
                                     "messages": [
                                         {
                                             "role": "user",
                                             "content": "What is the capital of the France?"
                                         }
                                     ],
                                     "temperature": 0.7
                                 }
                                 """;

        var actualRequest = _server.LogEntries.First().RequestMessage.Body!;
        _server.LogEntries.Should().Contain(x => x.RequestMessage.Path == "/v1/chat/completions");
        var actual = JToken.Parse(actualRequest);
        var expected = JToken.Parse(expectedRequest);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CapitalOfFranceWithTopP()
    {
        await _completionCaller(new ChatRequest
        {
            Model = "gpt-3.5-turbo",
            TopP = 1,
            Messages = new List<Message>
            {
                new()
                {
                    Role = "user",
                    Content = "What is the capital of the France?",
                }
            }
        });

        string expectedRequest = """
                                 {
                                     "model": "gpt-3.5-turbo",
                                     "messages": [
                                         {
                                             "role": "user",
                                             "content": "What is the capital of the France?"
                                         }
                                     ],
                                     "top_p": 1
                                 }
                                 """;

        var actualRequest = _server.LogEntries.First().RequestMessage.Body!;
        _server.LogEntries.Should().Contain(x => x.RequestMessage.Path == "/v1/chat/completions");
        var actual = JToken.Parse(actualRequest);
        var expected = JToken.Parse(expectedRequest);
        actual.Should().BeEquivalentTo(expected);
    }


    public void Dispose()
    {
        _server.Dispose();
    }
}