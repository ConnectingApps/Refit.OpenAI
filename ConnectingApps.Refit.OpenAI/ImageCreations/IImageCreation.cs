using Refit;
using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.ImageCreations.Request;
using ConnectingApps.Refit.OpenAI.ImageCreations.Response;

namespace ConnectingApps.Refit.OpenAI.ImageCreations
{
    public interface IImageCreation
    {
        [Post("/v1/images/generations")]
        Task<ApiResponse<ImageCreationResponse>> CreateModerationAsync([Body] ImageCreationRequest moderationRequest,
            [Header("Authorization")] string authorization);
    }
}
