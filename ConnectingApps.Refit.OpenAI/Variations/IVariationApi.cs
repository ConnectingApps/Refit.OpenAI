using System.Collections.Generic;
using System.Threading.Tasks;
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

    public class VariationResponse
    {
        public long Created { get; set; }
        public List<Variation> Data { get; set; }
    }

    public class Variation
    {
        public string Url { get; set; }
    }
}
