using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Embeddings.Request;
using ConnectingApps.Refit.OpenAI.Embeddings.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Embeddings
{
    public interface IEmbedding
    {
        [Post("/v1/embeddings")]
        Task<ApiResponse<EmbeddingResponse>> GetEmbeddingAsync([Body] EmbeddingRequest request,
            [Header("Authorization")] string authorization);
    }
}
