using System.Net.Http.Json;
using SunPro_Jobs.Models;
using Polly;
using Polly.Retry;

namespace SunPro_Jobs.Service
{
    public class ApplicationService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private const string BaseUrl = "https://localhost:7068/api/Application";

        public ApplicationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(2));

        }

        public async Task<bool> ApplyJobAsync(MultipartFormDataContent content)
        {
            Console.WriteLine("Sending request to ApplyJob endpoint.");
          //  _httpClient.Timeout = TimeSpan.FromMinutes(10);  // Ensure a longer timeout

            using var response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsync($"{BaseUrl}/ApplyJob", content));

            Console.WriteLine($"Response status: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }



        public async Task<List<ApplicationModel>> GetAllApplicationsAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/GetAllApplications");
            if (response.IsSuccessStatusCode)
            {
                var applications = await response.Content.ReadFromJsonAsync<List<ApplicationModel>>();
                Console.WriteLine($"API returned {applications?.Count} applications.");
                return applications ?? new List<ApplicationModel>();
            }
            Console.WriteLine($"Failed to retrieve applications. Status Code: {response.StatusCode}");
            throw new Exception("Failed to retrieve applications.");
        }


        public async Task<ApplicationModel> GetApplicationByIdAsync(int ApplicationId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/GetApplication/{ApplicationId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApplicationModel>();
            }
            throw new Exception($"Application with ID {ApplicationId} not found.");
        }

        public async Task<bool> UpdateApplicationAsync(int applicationId, ApplicationModel updatedApplication)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/UpdateApplication/{applicationId}", updatedApplication);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteApplicationAsync(int applicationId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/Delete/{applicationId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> HasUserAlreadyAppliedAsync(int userId, int jobId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/HasUserApplied?userId={userId}&jobId={jobId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            throw new Exception("Error checking application status.");
        }

        public async Task<bool> GenerateTemporaryCredentialsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/GenerateTemporaryCredentials?UserId={userId}", null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send credentials: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending temporary credentials: {ex.Message}");
                throw;
            }
        }
    }
}
