using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Modeling;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var modelingApi = RestService.For<IModeling>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);
var authHeader = $"Bearer {apiKey}";

var modelsResponse = await modelingApi.GetModelsAsync(authHeader);
Console.WriteLine($"Models response {modelsResponse.StatusCode}");
Console.WriteLine($"Id of first model {modelsResponse.Content!.Data.First().Id}" );

var modelResponse = await modelingApi.GetModelAsync(authHeader, modelsResponse.Content!.Data.First().Id);
Console.WriteLine($"Model response {modelResponse.StatusCode}");
Console.WriteLine($"Object name of model {modelResponse.Content!.Object}" );
Console.WriteLine($"OwnedBy model {modelResponse.Content!.OwnedBy}" );

var deleteResponse = await modelingApi.DeleteModelAsync(authHeader, "unknownName");
Console.WriteLine($"Statuscode of delete response {deleteResponse.StatusCode}");