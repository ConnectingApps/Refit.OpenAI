using System.Net;
using ConnectingApps.Refit.OpenAI.Modeling;
using FluentAssertions;
using Refit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest;

public class ModelingMockTest : IDisposable
{
    private readonly IModeling _modelingApi;
    private readonly string _authToken;
    private readonly WireMockServer _wireMockServer;

    public ModelingMockTest()
    {
        _wireMockServer = WireMockServer.Start();
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
        apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
        _authToken = $"Bearer {apiKey}";
        _modelingApi = RestService.For<IModeling>(_wireMockServer.Url!, OpenAiRefitSettings.RefitSettings);
    }

    private void SetupDelete(string toRespond)
    {
        _wireMockServer.Given(Request.Create().UsingDelete())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(toRespond)
            );
    }

    [Theory]
    [InlineData("""
                {
                  "id": "ft:gpt-3.5-turbo:acemeco:suffix:abc124",
                  "object": "model",
                  "deleted": true
                }
                """
                , true, "model", "ft:gpt-3.5-turbo:acemeco:suffix:abc124")]
    public async Task TryDelete(string toRespond, 
        bool expectedDelete,
        string expectedObject,
        string expectedId)
    {
        SetupDelete(toRespond);
        var deleteResponse = await _modelingApi.DeleteModelAsync(_authToken, "Something");
        (deleteResponse.StatusCode, deleteResponse.Error?.Content).Should().Be((HttpStatusCode.OK, null));
        (deleteResponse.Content?.Deleted,
                deleteResponse.Content?.Object,
                deleteResponse.Content?.Id)
            .Should().Be((expectedDelete, expectedObject, expectedId));
    }

    public void Dispose()
    {
        _wireMockServer.Stop();
        _wireMockServer.Dispose();
    }
}