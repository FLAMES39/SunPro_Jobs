using System.Net.Http.Json;
using SunPro_Jobs.Models;
using Polly;
using Polly.Retry;

namespace SunPro_Jobs.Service
{
    public class JobService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private const string BaseUrl = "https://localhost:7068/api/Jobs";

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(3, onRetry: (response, retryCount) =>
                {
                    Console.WriteLine($"Retry {retryCount} for {response.Result.StatusCode}");
                });
        }

        public async Task<List<JobModel>> GetAllJobsAsync()
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{BaseUrl}/GetAllJobs"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<JobModel>>() ?? new List<JobModel>();
            }
            throw new Exception($"Failed to retrieve jobs. Status Code: {response.StatusCode}");
        }

        public async Task<JobModel> GetSingleJobAsync(int jobId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/GetSingleJob/{jobId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<JobModel>();
            }
            throw new Exception($"Job with ID {jobId} not found.");
        }

        public async Task<JobModel> CreateJobAsync(JobModel job)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/postjob", job);
            if (response.IsSuccessStatusCode)
            {
                var createdJob = await response.Content.ReadFromJsonAsync<JobModel>();
                return createdJob;  // Return the full JobModel with Jobink
            }
            throw new Exception($"Failed to create job. Status Code: {response.StatusCode}");
        }



        public async Task<bool> UpdateJobAsync(int jobId, JobModel job)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/updateJobPost/{jobId}", job);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/DeleteJob/{jobId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<JobModel>> SearchJobsAsync(JobSearchModel searchCriteria)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/SearchJobs", searchCriteria);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<JobModel>>() ?? new List<JobModel>();
            }
            throw new Exception("No jobs found matching the search criteria.");
        }

        public async Task<JobModel> GetJobLinkAsync(string jobink)
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{BaseUrl}/GetJobLink/{jobink}"));
            if (response.IsSuccessStatusCode)
            {
                var job = await response.Content.ReadFromJsonAsync<JobModel>();
                if (job != null)
                {
                    return job;
                }
                throw new Exception("No job found for the specified link.");
            }
            throw new Exception($"Failed to retrieve job link. Status Code: {response.StatusCode}");
        }
    }
}
