using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Variations
{
    public interface IVariationApi
    {
        [Post("/v1/images/variations")]
        Task<ApiResponse<GenerateImageVariationsResponse>> GenerateImageVariations(
            [Header("Authorization")] string authorizationHeader,
            [Body] GenerateImageVariationsRequest request);
    }

    public class GenerateImageVariationsRequest
    {
        public Stream Image { get; set; }
        public int N { get; set; }
        public string Size { get; set; }
    }

    public class GenerateImageVariationsResponse
    {
        public long Created { get; set; }
        public List<GenerateImageVariationResponse> Data { get; set; }
    }

    public class GenerateImageVariationResponse
    {
        public string Url { get; set; }
    }

}
