using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Variations;
using Refit;

Console.WriteLine("Hello, World!");

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
var image = new FileStream("otter.png", FileMode.Open, FileAccess.Read);

var openAiApi = RestService.For<IVariationApi>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);

var exist = File.Exists("otter.png");
Console.WriteLine($"File exists: {exist}");
var streamPart = new StreamPart(image, "otter.png");

try
{
    var response = await openAiApi.GetImageVariations(authorizationHeader, streamPart, 2, "1024x1024");

    Console.WriteLine($"Returned response status code {response.StatusCode}");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


