using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.FineTune.Request;
using ConnectingApps.Refit.OpenAI.FineTune.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.FineTune
{
    public interface IFineTune
    {
        [Post("/v1/fine_tuning/jobs")]
        public Task<ApiResponse<FineTuneResponse>> FineTuneAsync([Body] FineTuneRequest request,
            [Header("Authorization")] string authorization);
    }
}
