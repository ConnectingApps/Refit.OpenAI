using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Completions.Request;
using ConnectingApps.Refit.OpenAI.Completions.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Completions
{
    public interface ICompletion
    {
        [Post("/v1/chat/completions")]
        Task<ApiResponse<ChatResponse>> CreateCompletionAsync([Body] ChatRequest chatRequest, [Header("Authorization")] string authorization);
    }
}
