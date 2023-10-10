# Refit.OpenAI
ConnectingApps.Refit.OpenAI is a [Refit](https://github.com/reactiveui/refit#refit-the-automatic-type-safe-rest-library-for-net-core-xamarin-and-net) client package for calling OpenAI (but not made by the OpenAI company). Using this package, you can call the OpenAI API while being in full control of resilience en logging. This is because Refit is used so you can be fully control the `HttpClient`, including the logging, returned HTTP status codes etc.

For example, assuming you want to do this HTTP call:

```http
POST https://api.openai.com/v1/chat/completions
Content-Type: application/json
Authorization: Bearer {{key}}
```
```json
{
    "model": "gpt-3.5-turbo",
    "messages": [
        {
            "role": "user",
            "content": "What is the average temperature in New York city in January?"
        }
    ],
    "temperature": 0.7
}
```


you can achieve that with the following C# code:

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Completions;
using ConnectingApps.Refit.OpenAI.Completions.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var completionApi = RestService.For<ICompletion>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);

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
```

giving the following output:

```cmd
Returned response status code OK
The capital of France is Paris.
```

You can reuse your existing Refit experience to:
1. Implement resilience in case of accidental failures
2. Trace the statuscode of your REST Calls
3. Code in C# like it is a raw request
4. Benefit from the highly performant (de)serialization using [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/#readme-body-tab)
