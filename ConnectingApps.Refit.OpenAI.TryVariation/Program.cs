using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Variations;
using Refit;

Console.WriteLine("Hello, World!");

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
var image = new FileStream("otter.png", FileMode.Open, FileAccess.Read);

var openAiApi = RestService.For<IVariationApi>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);

try
{
    var response = await openAiApi.GenerateImageVariations(authorizationHeader, new GenerateImageVariationsRequest()
    {
        N = 2,
        Size = "1024x1024",
        Image = image
    });
    Console.WriteLine($"Returned response status code {response.StatusCode}");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


