using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Variations;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var image = new FileStream("otter.png", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IVariationApi>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var exist = File.Exists("otter.png");
    Console.WriteLine($"File exists: {exist}");
    var streamPart = new StreamPart(image, "otter.png");
    var response = await openAiApi.GetImageVariations(authorizationHeader, streamPart, 2, "1024x1024");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Number of new items created {response.Content!.Data.Count}");
    Console.WriteLine($"First item url {response.Content!.Data.First().Url}");
    Console.WriteLine($"Second item url {response.Content!.Data.Last().Url}");
}
