using System.Net;
using ConnectingApps.Refit.OpenAI.Files;
using ConnectingApps.Refit.OpenAI.FineTune;
using ConnectingApps.Refit.OpenAI.FineTune.Request;
using FluentAssertions;
using Refit;


namespace ConnectingApps.Refit.OpenAI.IntegrationTest
{
    public class FineTuneTest
    {
        private static readonly IFiles FileApi;
        private static readonly IFineTune FineTuneApi;
        private static readonly string ApiKey;

        static FineTuneTest()
        {
            ApiKey = Environment.GetEnvironmentVariable("OPENAI_KEY")!;
            ApiKey.Should().NotBeNullOrEmpty("OPENAI_KEY environment variable must be set");
            FileApi = RestService.For<IFiles>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
            FineTuneApi = RestService.For<IFineTune>("https://api.openai.com", OpenAiRefitSettings.RefitSettings);
        }

        [Fact]
        public async Task TryToList()
        {
            var jobs = await FineTuneApi.GetJobsAsync($"Bearer {ApiKey}");
            (jobs.Error?.Content, jobs.StatusCode).Should().Be((null, HttpStatusCode.OK));
            jobs.Content!.Object.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task TryToListLimit()
        {
            var jobs = await FineTuneApi.GetJobsAsync($"Bearer {ApiKey}", 2);
            (jobs.Error?.Content, jobs.StatusCode).Should().Be((null, HttpStatusCode.OK));
            jobs.Content!.Object.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, 1)]
        [InlineData(HttpStatusCode.BadRequest, 18000)]
        public async Task TryToPost(HttpStatusCode expectStatusCodeOfCancelRespons, int waitingTimeBeforeCancel)
        {
            await using (var fineTuneData = new FileStream("fineTune.jsonl", FileMode.Open, FileAccess.Read))
            {
                var streamPart = new StreamPart(fineTuneData, "fineTune.jsonl");
                var postFileResponse = await FileApi.PostFileAsync($"Bearer {ApiKey}", streamPart, "fine-tune");
                (postFileResponse.Error?.Content, postFileResponse.StatusCode).Should().Be((null, HttpStatusCode.OK));
                var postJobResponse = await FineTuneApi.PostJob(new FineTuneRequest
                {
                    Model = "gpt-3.5-turbo",
                    TrainingFile = postFileResponse.Content!.Id
                }, $"Bearer {ApiKey}");
                (postJobResponse.Error?.Content, postJobResponse.StatusCode).Should().Be((null, HttpStatusCode.OK));
                postJobResponse.Content!.Object.Should().NotBeNullOrEmpty();

                var getJobResponse = await FineTuneApi.GetJobAsync(postJobResponse.Content!.Id, $"Bearer {ApiKey}");
                (getJobResponse.Error?.Content, getJobResponse.StatusCode).Should().Be((null, HttpStatusCode.OK));

                var getJobsResponse = await FineTuneApi.GetJobsAsync($"Bearer {ApiKey}");
                (getJobsResponse.Error?.Content, getJobsResponse.StatusCode).Should().Be((null, HttpStatusCode.OK));
                getJobsResponse.Content!.Data.Should().NotBeEmpty();

                await Task.Delay(waitingTimeBeforeCancel);
                var cancelJobResponse = await FineTuneApi.CancelJobAsync(postJobResponse.Content.Id, $"Bearer {ApiKey}");
                cancelJobResponse.StatusCode.Should().Be(expectStatusCodeOfCancelRespons);
                $"{cancelJobResponse.Content?.Id} {cancelJobResponse.Error?.Content}".Should().NotBeEmpty();
            }
        }
    }
}
