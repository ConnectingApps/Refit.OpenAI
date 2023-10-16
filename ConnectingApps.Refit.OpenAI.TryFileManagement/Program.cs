using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Files;
using ConnectingApps.Refit.OpenAI.Files.Response;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
var completionApi = RestService.For<IFiles>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var getResponse = await completionApi.GetFilesAsync(authorizationHeader);
Console.WriteLine($"Returned GET response status code {getResponse.StatusCode}");
Console.WriteLine($"Number of items {getResponse.Content!.Data.Length}");

ApiResponse<FilePostResponse> postResponse;

await using (var image = new FileStream("mydata.jsonl", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IFiles>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(image, "mydata.jsonl");
    postResponse = await openAiApi.PostFileAsync(authorizationHeader, streamPart, "fine-tune");
    Console.WriteLine($"Returned POST response status code {postResponse.StatusCode}");
    Console.WriteLine($"Returned POST response number of bytes {postResponse.Content!.Bytes}");
}

var newGetResponse = await completionApi.GetFilesAsync(authorizationHeader);
Console.WriteLine($"Returned GET response status code after POST {newGetResponse.StatusCode}");
Console.WriteLine($"Number of items {newGetResponse.Content!.Data.Length}");

await Task.Delay(10000);
var deleteResponse = await completionApi.DeleteFileAsync(postResponse.Content.Id, authorizationHeader);
Console.WriteLine($"Returned DELETE response status code {deleteResponse.StatusCode}");

var deleteAfterGetResponse = await completionApi.GetFilesAsync(authorizationHeader);
Console.WriteLine($"Returned GET response status code after DELETE {deleteAfterGetResponse.StatusCode}");
Console.WriteLine($"Number of items {deleteAfterGetResponse.Content!.Data.Length}");