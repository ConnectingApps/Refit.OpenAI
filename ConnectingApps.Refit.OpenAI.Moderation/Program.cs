using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Moderations;
using ConnectingApps.Refit.OpenAI.Moderations.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var moderationApi = RestService.For<IModeration>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await moderationApi.CreateModerationAsync(new ModerationRequest
    {
        Input = "I want to hit my dog."
    }, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine($"Check if this is violent {response.Content!.Results[0].Categories.Violence}");
