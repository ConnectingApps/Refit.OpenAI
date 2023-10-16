using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.Files.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.Files
{
    public interface IFiles
    {
        [Get("/v1/files")]
        Task<ApiResponse<FileGetResponse>> GetFilesAsync([Header("Authorization")] string authorizationToken);

        [Delete("/v1/files/{fileId}")]
        Task<ApiResponse<FileDeleteResponse>> DeleteFileAsync(string fileId, [Header("Authorization")] string authorizationToken);

        [Multipart]
        [Post("/v1/files")]
        Task<ApiResponse<FilePostResponse>> PostFileAsync(
            [Header("Authorization")] string authorization,
            [AliasAs("file")] StreamPart file,
            [AliasAs("purpose")] string purpose);
    }
}
