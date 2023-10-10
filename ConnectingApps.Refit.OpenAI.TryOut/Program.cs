using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Completions;
using ConnectingApps.Refit.OpenAI.Completions.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var completionApi = RestService.For<ICompletion>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await completionApi.CreateCompletionAsync(new ChatRequest
    {
        Model = "gpt-3.5-turbo",
        Temperature = 0.7,
        Messages = new List<Message>
        {
            new()
            {
                Role = "user",
                Content = "What is the capital of the France?",
            }
        }
    }, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine(response.Content!.Choices!.First().Message!.Content);
