using Refit;
using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Moderations.Response;
using ConnectingApps.Refit.OpenAI.Moderations.Request;

namespace ConnectingApps.Refit.OpenAI.Moderations
{
    public interface IModeration
    {
        [Post("/v1/moderations")]
        Task<ApiResponse<ModerationsResponse>> CreateModerationAsync([Body] ModerationRequest moderationRequest,
            [Header("Authorization")] string authorization);
    }
}
