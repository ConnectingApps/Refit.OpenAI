using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Files;
using Refit;


var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var completionApi = RestService.For<IFiles>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var getResponse = await completionApi.GetFilesAsync( $"Bearer {apiKey}");

Console.WriteLine($"Returned GET response status code {getResponse.StatusCode}");
Console.WriteLine($"Number of items {getResponse.Content!.Data.Length}");