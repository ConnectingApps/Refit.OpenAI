﻿using System.Threading.Tasks;
using ConnectingApps.Refit.OpenAI.FineTune.Request;
using ConnectingApps.Refit.OpenAI.FineTune.Response;
using Refit;

namespace ConnectingApps.Refit.OpenAI.FineTune
{
    public interface IFineTune
    {
        [Get("/v1/fine_tuning/jobs")]
        public Task<ApiResponse<FineTuneOverview>> GetJobsAsync( 
            [Header("Authorization")] string authorization, [Query("limit")] int? limit = null);

        [Post("/v1/fine_tuning/jobs")]
        public Task<ApiResponse<FineTuneResponse>> PostJobAsync([Body] FineTuneRequest request,
            [Header("Authorization")] string authorization);

        [Get("/v1/fine_tuning/jobs/{jobId}")]
        public Task<ApiResponse<FineTuneResponse>> GetJobAsync(string jobId, [Header("Authorization")] string authorization);

        [Post("/v1/fine_tuning/jobs/{jobId}/cancel")]
        public Task<ApiResponse<FineTuneResponse>> CancelJobAsync(string jobId, [Header("Authorization")] string authorization);
    }
}
