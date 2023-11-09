using FluentAssertions;
using FluentAssertions.Json;
using Refit;
using System.Net;
using ConnectingApps.Refit.OpenAI.Files;
using Newtonsoft.Json.Linq;

namespace ConnectingApps.Refit.OpenAI.IntegrationTest;

public class FileTest
{
    private static readonly IFiles FileApi;
    private static readonly string ApiKey;

    static FileTest()
    {
        ApiKey = Environment.GetEnvironmentVariable("OPENAI_KEY")!;
        ApiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
        FileApi = RestService.For<IFiles>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
    }

    [Fact]
    public async Task GetFiles()
    {
        var response = await FileApi.GetFilesAsync($"Bearer {ApiKey}");
        (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
        response.Content!.Object.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PostFile()
    {
        await using (var image = new FileStream("mydata.jsonl", FileMode.Open, FileAccess.Read))
        {
            var streamPart = new StreamPart(image, "mydata.jsonl");
            var postResponse = await FileApi.PostFileAsync($"Bearer {ApiKey}", streamPart, "fine-tune");
            (postResponse.Error?.Content, 
                    postResponse.StatusCode,
                    postResponse.Content?.Bytes is > 480 and < 500,
                    postResponse.Content?.Filename,
                    postResponse.Content?.Status,
                    postResponse.Content?.StatusDetails
                ).Should().
                Be((null,
                    HttpStatusCode.OK,
                    true,
                    "mydata.jsonl",
                    "uploaded",
                    null));
        }
    }

    [Fact]
    public async Task DeleteKnownFile()
    {
        var response = await FileApi.GetFilesAsync($"Bearer {ApiKey}");
        (response.Error?.Content, response.StatusCode).Should().Be((null, HttpStatusCode.OK));
        response.Content!.Object.Should().NotBeNullOrEmpty();
        await using (var image = new FileStream("mydata.jsonl", FileMode.Open, FileAccess.Read))
        {
            var streamPart = new StreamPart(image, "mydata.jsonl");
            var postResponse = await FileApi.PostFileAsync($"Bearer {ApiKey}", streamPart, "fine-tune");
            var getResponseAfterPost = await FileApi.GetFilesAsync($"Bearer {ApiKey}");
            await Task.Delay(10000);
            var deleteResponse = await FileApi.DeleteFileAsync(postResponse.Content?.Id!, $"Bearer {ApiKey}");
            var getResponseAfterDelete = await FileApi.GetFilesAsync($"Bearer {ApiKey}");

            (getResponseAfterPost.Error?.Content,
                    getResponseAfterPost.StatusCode,
                    deleteResponse.Error?.Content,
                    deleteResponse.StatusCode,
                    getResponseAfterDelete.Error?.Content,
                    getResponseAfterDelete.StatusCode,
                    postResponse.Error?.Content,
                    postResponse.StatusCode)
                .Should().Be(
                    (null,
                        HttpStatusCode.OK,
                        null,
                        HttpStatusCode.OK,
                        null,
                        HttpStatusCode.OK,
                        null,
                        HttpStatusCode.OK));

            getResponseAfterDelete.Content!.Data.Length.Should().Be(getResponseAfterPost.Content!.Data.Length - 1);
            getResponseAfterDelete.Content!.Data.Should().NotContain(x => x.Id == postResponse.Content!.Id);
            getResponseAfterPost.Content!.Data.Should().Contain(x => x.Id == postResponse.Content!.Id);
            response.Content!.Data.Should().NotContain(x => x.Id == postResponse.Content!.Id);
        }
    }

    [Fact]
    public async Task DeleteUnknownFile()
    {
        var response = await FileApi.DeleteFileAsync("file-FZ3UiIdfjYFzAsooLLUtu01F", $"Bearer {ApiKey}");
        string expectedErrorMessage = """
                                      {
                                        "error": {
                                          "message": "No such File object: file-FZ3UiIdfjYFzAsooLLUtu01F",
                                          "type": "invalid_request_error",
                                          "param": "id",
                                          "code": null
                                        }
                                      }
                                      """;
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Error!.Content!.Should().NotBeNullOrEmpty();
        var actual = JToken.Parse(response.Error!.Content!);
        var expected = JToken.Parse(expectedErrorMessage);
        actual.Should().BeEquivalentTo(expected);
    }
}