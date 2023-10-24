# Refit.OpenAI
**ConnectingApps.Refit.OpenAI** is a [Refit](https://github.com/reactiveui/refit#refit-the-automatic-type-safe-rest-library-for-net-core-xamarin-and-net) client package designed for invoking OpenAI (note: this is not developed by the OpenAI company). Utilizing this package allows you to make calls to the OpenAI API while maintaining complete control over resilience and logging. This is possible because Refit enables thorough control over the `HttpClient`, including logging, returned HTTP status codes, and more. OpenAI already [clearly documented](https://platform.openai.com/docs/api-reference/making-requests) their API and how to call it with `curl` or `python`. This is document explains how you can use this NuGet package do to the same with C#.

# Table of Contents
- [Refit.OpenAI](#refitopenai)
- [Table of Contents](#table-of-contents)
- [Features](#features)
  - [Completions](#completions)
  - [Image Variations](#image-variations)
  - [Audio Translations](#audio-translations)
  - [Audio Transcriptions](#audio-transcriptions)
  - [Moderations](#moderations)
  - [Image Creations](#image-creations)
  - [File Management](#file-management)
  - [Fine-tuning](#fine-tuning)
  - [Embeddings](#embeddings)
- [Why Refit](#why-refit)
- [Legal Statement](#legal-statement)
  - [Limitation of Liability](#limitation-of-liability)
  - [Miscellaneous](#miscellaneous)

# Features

## Completions

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
            "content": "What is the capital of the France?"
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

## Image Variations

In addition to the completions functionality in OpenAI, this NuGet package also supports image variations.

Such an image variation can be achieved with this curl call.

```bash
curl https://api.openai.com/v1/images/variations \
  -H "Authorization: Bearer YOURKEY" \
  -F image="@otter.png" \
  -F n=2 \
  -F size="1024x1024"
```

This how to run such a call in C#:

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Variations;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var image = new FileStream("otter.png", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IVariation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(image, "otter.png");
    var response = await openAiApi.GetImageVariations(authorizationHeader, streamPart, 2, "1024x1024");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Number of new items created {response.Content!.Data.Count}");
    Console.WriteLine($"First item url {response.Content!.Data.First().Url}");
    Console.WriteLine($"Second item url {response.Content!.Data.Last().Url}");
}
```

giving the following output:

```cmd
Returned response status code OK
Number of new items created 2
First item url https://oaidalleapiprodscus.blob.core.windows.net/private/org-Rw9eshPWEaNfb.....[REST OF IMAGE URL 1]
Second item url https://oaidalleapiprodscus.blob.core.windows.net/private/org-Rw9eshPWEaQ.....[REST OF IMAGE URL 2]
```

## Audio Translations

OpenAI API does support audio translations and so this this NuGet package. Here is an example of a curl call to request an audio translation from Dutch to English:

```bash
curl https://api.openai.com/v1/audio/translations \
  -H "Authorization: Bearer YOURKEY" \
  -H "Content-Type: multipart/form-data" \
  -F file="@HalloWereld.mp3" \
  -F model="whisper-1"
```

This is how to code this in C#:

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.AudioTranslation;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var recording = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<IAudioTranslation>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(recording, "HalloWereld.mp3");
    var response = await openAiApi.GetAudioTranslation(authorizationHeader, streamPart, "whisper-1");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Translated text {response.Content!.Text}");
}
```

giving the following output:

```cmd
Returned response status code OK
Translated text Hello world, the world is mine.
```

## Audio Transcriptions

OpenAI API does support audio transcriptions and so this this NuGet package. You'll get the actual text, even when it is not in English.

Here is an example of a curl call to request an audio transcription of a recording in Dutch:

```bash
curl https://api.openai.com/v1/audio/transcriptions \
  -H "Authorization: Bearer YOURKEY" \
  -H "Content-Type: multipart/form-data" \
  -F file="@HalloWereld.mp3" \
  -F model="whisper-1"
```

This is how to code this in C#:

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Transcriptions;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var authorizationHeader = $"Bearer {apiKey}";
await using (var recording = new FileStream("HalloWereld.mp3", FileMode.Open, FileAccess.Read))
{
    var openAiApi = RestService.For<ITranscription>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    var streamPart = new StreamPart(recording, "HalloWereld.mp3");
    var response = await openAiApi.GetAudioTranscription(authorizationHeader, streamPart, "whisper-1");
    Console.WriteLine($"Returned response status code {response.StatusCode}");
    Console.WriteLine($"Actual text {response.Content!.Text}");
}
```
giving the following output:

```cmd
Returned response status code OK
Actual text Hallo wereld de wereld is van mij
```

## Moderations
A common problem on public websites is that they need to be moderated but the organizations that are supposed to do that are understaffed. For such problems, the OpenAI API can help which can be easily called through this NuGet package.

Assume you want to do this request:

```http
POST https://api.openai.com/v1/moderations
Content-Type: application/json
Authorization: Bearer YOURKEY
```
```json
{
    "input": "I want to hit my dog."
}
```

You can run this using the following C# code:

```csharp
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
```

giving this output:

```txt
Returned response status code OK
Check if this is violent True
```

In this way, you can check for inappropriate comments given by users of your website.

## Image Creations

For many years, companies had to hire photographers and graphical designers for all their stock images. Nowadays, companies can choose themselves if they want a human created or an AI created stock image. This is because OpenAI supports image creation based on a description and so does this NuGet package. Here is an example of a curl call you may want to do to generate two images, returned as urls for downloading, of a baby sea otter:

```http
POST https://api.openai.com/v1/images/generations
Content-Type: application/json
Authorization: Bearer YOURKEY
```
```json
{
    "prompt": "A cute baby sea otter",
    "n": 2,
    "size": "1024x1024"
}
```

Here is how to code this in C#:

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.ImageCreations;
using ConnectingApps.Refit.OpenAI.ImageCreations.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var creationApi = RestService.For<IImageCreation>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await creationApi.CreateImageAsync(new ImageCreationRequest
    {
        N = 2,
        Prompt = "A cute baby sea otter.",
        Size = "1024x1024",
    }, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine($"Number of urls {response.Content!.Data.Length}" );
Console.WriteLine($"First url {response.Content!.Data.First().Url}");
Console.WriteLine($"Last url {response.Content!.Data.Last().Url}");
```

Here is the output:

```text
Returned response status code OK
Number of urls 2
First url [FIRST URL]
Last url [SECOND URL]
```

## File Management

In various situations, you may want to post files which you can also delete. For example, the files you use for fine-tuning your models. Like the OpenAI API, this NuGet package supports that. Here is the C# code to demonstrate that.


```csharp
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
```

which gives the following output:
```text
Returned GET response status code OK
Number of items 20
Returned POST response status code OK
Returned POST response number of bytes 493
Returned GET response status code after POST OK
Number of items 21
Returned DELETE response status code OK
Returned GET response status code after DELETE OK
Number of items 20
```

which shows that the number of files goes up after posting a new one and goes down after deleting an existing one.

## Fine-tuning

After uploading a file that can be used for fine-tuning, you can do the fine-tuning itself by starting (and then canceling a job). Here is how:

```csharp
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
```

This gives the following output

```txt
Returned response status code OK
Number of jobs 42
Returned POST response status code OK
Returned POST response number of bytes 246
New job response OK
New Job files Returned response status code OK
Number of jobs after POST 43
Get new job response OK
Cancel job response OK
```

This shows the number of jobs goes up after a new job is posted.

## Embeddings
The relatedness of text strings can be measured using OpenAI’s text embeddings. This is relevant for things such as searching, clustering and classification. To learn more about this topic, read the [OpenAI documentation](https://platform.openai.com/docs/guides/embeddings/what-are-embeddings) about it. 

Here is an example of a call get an embedding:

```http
POST https://api.openai.com/v1/embeddings
Authorization: Bearer {{key}}
Content-Type: application/json
```
```json
{
    "input": "The food was delicious",
    "model": "text-embedding-ada-002"
}
```

This can be coded in C# using our NuGet package.

```csharp
using ConnectingApps.Refit.OpenAI;
using ConnectingApps.Refit.OpenAI.Embeddings;
using ConnectingApps.Refit.OpenAI.Embeddings.Request;
using Refit;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
var completionApi = RestService.For<IEmbedding>(new HttpClient
{
    BaseAddress = new Uri("https://api.openai.com")
}, OpenAiRefitSettings.RefitSettings);

var response = await completionApi.GetEmbeddingAsync(new EmbeddingRequest
    {
        Input = "The food was delicious",
        Model = "text-embedding-ada-002"
}, $"Bearer {apiKey}");

Console.WriteLine($"Returned response status code {response.StatusCode}");
Console.WriteLine($"Vector length {response.Content!.Data.First().Embedding.Length}");
Console.WriteLine($"Vector {string.Join(", ", response.Content!.Data.First().Embedding.Take(10))}...");
```

This gives the following output:

```text
Returned response status code OK
Vector length 1536
Vector 0,022599462, -0,0008510616, -0,005139073, -0,010128645, -0,0023203692, 0,0057370737, -0,01077648, -0,03453457, 0,008851663, -0,044576004...
```

# Why Refit
You can reuse your existing Refit experience to:
1. Implement resilience in case of accidental failures
2. Trace the HTTP status code of your REST Calls
3. Code in C# like it is a raw request
4. Benefit from the highly performant (de)serialization using [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/#readme-body-tab)


# Legal Statement
This Software incorporates certain components that are not owned by Connecting Apps. These external components, including but not limited to software libraries, plugins, modules, or any other type of software components (collectively, “External Components”), are the property of their respective owners and are used within the Software either under license or as allowed by applicable law.

Each of these External Components is subject to its own applicable license terms and conditions, which may be found in the documentation accompanying the External Components, in the External Components’ respective official repositories, or through other appropriate channels provided by the respective owners or licensors of the External Components. Users of the Software are responsible for complying with those terms and conditions, as well as with any and all other applicable laws and regulations related to the use of the External Components.

The Company disclaims any proprietary interests in the intellectual property of the External Components. The inclusion of these External Components within the Software does not imply any endorsement, affiliation, or sponsorship between the Company and the respective owners or licensors of the External Components.

## Limitation of Liability
To the maximum extent permitted by applicable law, the Company shall not be liable for any damages arising out of or in connection with the use of the External Components incorporated within the Software. Users agree to hold harmless and indemnify the Company against any claims or liabilities arising out of the use, reproduction, or distribution of the External Components.

## Miscellaneous
This Legal Statement may be updated from time to time at the sole discretion of the Company. Users are encouraged to review this Statement periodically to stay informed of any changes.

For any questions regarding this Legal Statement, or to obtain more information about the External Components incorporated within the Software, please contact us by clicking "Contact owners" on the NuGet page.