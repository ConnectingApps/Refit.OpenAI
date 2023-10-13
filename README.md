# Refit.OpenAI
**ConnectingApps.Refit.OpenAI** is a [Refit](https://github.com/reactiveui/refit#refit-the-automatic-type-safe-rest-library-for-net-core-xamarin-and-net) client package for calling OpenAI (but not made by the OpenAI company). Using this package, you can call the OpenAI API while being in full control of resilience and logging. This is because Refit is used so you can be fully control the `HttpClient`, including the logging, returned HTTP status codes etc.


# Table of Contents
- [Refit.OpenAI](#refitopenai)
- [Table of Contents](#table-of-contents)
- [Features](#features)
  - [Completions](#completions)
  - [Variations](#variations)
  - [Audio Translations](#audio-translations)
  - [Audio Transcriptions](#audio-transcriptions)
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

## Variations

In addition to the completions functionality in OpenAI, this NuGet package also support image variations.

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

# Why Refit
You can reuse your existing Refit experience to:
1. Implement resilience in case of accidental failures
2. Trace the statuscode of your REST Calls
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