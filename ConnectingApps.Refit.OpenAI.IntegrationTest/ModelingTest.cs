using System.Net;
using ConnectingApps.Refit.OpenAI.Modeling;
using FluentAssertions;
using Refit;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest;

public class ModelingTest
{
    private static readonly IModeling ModelingApi;
    private static readonly string AuthToken;

    static ModelingTest()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
        apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
        AuthToken = $"Bearer {apiKey}";
        ModelingApi = RestService.For<IModeling>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    }

    [Fact]
    public async Task TryDeleteNotFound()
    {
        var deleteResponse = await ModelingApi.DeleteModelAsync(AuthToken, "UnknownModel");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task TryGetAll()
    {
        var getmodelsResponse = await ModelingApi.GetModelsAsync(AuthToken);
        (getmodelsResponse.StatusCode, getmodelsResponse.Error?.Content).Should().Be((HttpStatusCode.OK, null));
    }
    
    [Fact]
    public async Task TryGetOne()
    {
        var getmodelsResponse = await ModelingApi.GetModelsAsync(AuthToken);
        (getmodelsResponse.StatusCode, getmodelsResponse.Error?.Content).Should().Be((HttpStatusCode.OK, null));
        var getModelResponse = await ModelingApi.GetModelAsync(AuthToken, 
            getmodelsResponse.Content!.Data.First().Id);
        (getModelResponse.StatusCode, getModelResponse.Error?.Content).Should().Be((HttpStatusCode.OK, null));
    }
}