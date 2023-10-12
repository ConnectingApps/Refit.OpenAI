using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.AudioTranslation.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.AudioTranslation
{
    public interface IAudioTranslation
    {
        [Multipart]
        [Post("/v1/audio/translations")]
        Task<ApiResponse<TranslationResponse>> GetAudioTranslation(
            [Header("Authorization")] string authorization,
            [AliasAs("file")] StreamPart image,
            [AliasAs("model")] string model);
    }
}
