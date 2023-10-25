using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Modeling.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Modeling
{
    public interface IModeling
    {
        [Get("/v1/models")]
        Task<ApiResponse<ModelingResponse>> GetModelsAsync([Header("Authorization")] string authorization);

        [Get("/v1/models/{model}")]
        Task<ApiResponse<Datum>> GetModelAsync([Header("Authorization")] string authorization, string model);
        
        [Delete("/v1/models/{model}")]
        Task<ApiResponse<DeleteResponse>> DeleteModelAsync([Header("Authorization")] string authorization, 
            string model);
    }
}
