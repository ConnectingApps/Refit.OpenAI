using FluentAssertions;
using Refit;
using System.Net;
using ConnectingApps.Refit.OpenAI.Embeddings;
using ConnectingApps.Refit.OpenAI.Embeddings.Request;
using ConnectingApps.Refit.OpenAI.Embeddings.Response;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class EmbeddingTest
    {
        private static readonly Func<EmbeddingRequest, Task<ApiResponse<EmbeddingResponse>>> EmbeddingCaller;

        static EmbeddingTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            apiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            var embedding = RestService.For<IEmbedding>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
            EmbeddingCaller = embeddingRequest => embedding.GetEmbeddingAsync(embeddingRequest, $"Bearer {apiKey}");
        }

        [Fact]
        public async Task CapitalOfFrance()
        {
            var response = await EmbeddingCaller(new EmbeddingRequest
            {
                Input = "The food was delicious",
                Model = "text-embedding-ada-002"
            });
            (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
            response.Content!.Data.First().Embedding.Length.Should().BeGreaterThan(1000);
            response.Content.Object.Should().Be("list");
            response.Content.Data.First().Index.Should().Be(0);
            response.Content.Data.First().Object.Should().Be("embedding");
        }
    }
}
