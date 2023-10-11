using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Variations.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Variations
{
    public interface IVariationApi
    {
        [Multipart]
        [Post("/v1/images/variations")]
        Task<ApiResponse<VariationResponse>> GetImageVariations(
            [Header("Authorization")] string authorization,
            [AliasAs("image")] StreamPart image,
            [AliasAs("n")] int n,
            [AliasAs("size")] string size);
    }

}
