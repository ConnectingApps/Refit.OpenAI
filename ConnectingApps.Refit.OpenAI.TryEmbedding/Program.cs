using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Embeddings;
using ConnectingApps.Refit.OpenAI.Embeddings.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var completionApi = RestService.For<IEmbedding>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await completionApi.GetEmbeddingAsync(new EmbeddingRequest
    {
        Input = "The food was delicious",
        Model = "text-embedding-ada-002"
}, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine($"Vector length {response.Content!.Data.First().Embedding.Length}");
Console.WriteLine($"Vector {string.Join(", ", response.Content!.Data.First().Embedding.Take(10))}...");
