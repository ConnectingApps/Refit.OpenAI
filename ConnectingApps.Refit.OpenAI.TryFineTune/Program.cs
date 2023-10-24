using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Files;
using ConnectingApps.Refit.OpenAI.FineTune;
using ConnectingApps.Refit.OpenAI.FineTune.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var fineTuneApi = RestService.For<IFineTune>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);
var token = $"Bearer {apiKey}";

var jobs = await fineTuneApi.GetJobsAsync(token, limit: 200);

Console.WriteLine($"Returned response status code {jobs.StatusCode}");
Console.WriteLine($"Number of jobs {jobs.Content!.Data.Length}");
string newTraingFile;

await using (var fineTuneDataStream = new FileStream("mydata.jsonl", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IFiles>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(fineTuneDataStream, "mydata.jsonl");
    var postFileResponse = await openAiApi.PostFileAsync(token, streamPart, "fine-tune");
    Console.WriteLine($"Returned POST response status code {postFileResponse.StatusCode}");
    Console.WriteLine($"Returned POST response number of bytes {postFileResponse.Content!.Bytes}");
    newTraingFile = postFileResponse.Content!.Id;
}

var newJobResponse = await fineTuneApi.PostJobAsync(new FineTuneRequest
{
    TrainingFile = newTraingFile,
    Model = "gpt-3.5-turbo"
}, token);

Console.WriteLine($"New job response {newJobResponse.StatusCode}");

var newJobs = await fineTuneApi.GetJobsAsync(token, limit:200);
Console.WriteLine($"New Job files Returned response status code {newJobs.StatusCode}");
Console.WriteLine($"Number of jobs after POST {newJobs.Content!.Data.Length}");

var newJob = await fineTuneApi.GetJobAsync(newJobResponse.Content!.Id, token);
Console.WriteLine($"Get new job response {newJob.StatusCode}");

var cancelResponse = await fineTuneApi.CancelJobAsync(newJobResponse.Content!.Id, token);
Console.WriteLine($"Cancel job response {cancelResponse.StatusCode}");

