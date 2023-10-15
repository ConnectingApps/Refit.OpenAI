using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.ImageCreations;
using ConnectingApps.Refit.OpenAI.ImageCreations.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var creationApi = RestService.For<IImageCreation>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await creationApi.CreateImageAsync(new ImageCreationRequest
    {
        N = 2,
        Prompt = "A cute baby sea otter.",
        Size = "1024x1024",
    }, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine($"Number of urls {response.Content!.Data.Length}" );
Console.WriteLine($"First url {response.Content!.Data.First().Url}");
Console.WriteLine($"Last url {response.Content!.Data.Last().Url}");
