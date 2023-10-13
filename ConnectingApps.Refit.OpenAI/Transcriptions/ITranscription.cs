using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Transcriptions.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Transcriptions
{
    public interface ITranscription
    {
        [Multipart]
        [Post("/v1/audio/transcriptions")]
        Task<ApiResponse<TranscriptionResponse>> GetAudioTranscription(
            [Header("Authorization")] string authorization,
            [AliasAs("file")] StreamPart image,
            [AliasAs("model")] string model);
    }
}
