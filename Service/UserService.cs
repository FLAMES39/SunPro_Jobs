using SunPro_Jobs.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;



namespace SunPro_Jobs.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        private IJSRuntime _jsruntime;

        private  const string BaseUrl = "https://localhost:7068";
        public UserService(HttpClient httpClient, IJSRuntime jsruntime)        
        {
            _httpClient = httpClient;
            _jsruntime = jsruntime;
        }

        public async Task<bool> RegisterUser(UserModel userModel)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", userModel);
            if(response == null)
            
                {
                    return false;
                }
            return response.IsSuccessStatusCode; ;
        }

        public async Task<bool> LoginUser (LoginModel loginModel)
        {
            var user = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginModel);
            if (user.IsSuccessStatusCode)
            {
                var responseContent = await user.Content.ReadFromJsonAsync<LoginResponse>();
                if (responseContent?.Token != null)
                {
                 await _jsruntime.InvokeVoidAsync("localStorage.setItem" , "authToken" , responseContent.Token);
                 await _jsruntime.InvokeVoidAsync("localStorage.setItem", "UserId", responseContent.UserId);
                 await _jsruntime.InvokeVoidAsync("localStorage.setItem", "Role", responseContent.Role);
                    

                return true;
                }
            }

            return false;
        }

        public async Task<string?> GetAuthToken()
        {
            return await _jsruntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
        }

        public async Task<bool> IsLogedIn()
        {
            var token = await GetAuthToken();
            return !string.IsNullOrEmpty(token);
        }

        public async Task Logout()
        {
            await _jsruntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsruntime.InvokeVoidAsync("localStorage.removeItem", "UserId");
            await _jsruntime.InvokeVoidAsync("localStorage.removeItem", "Role");
        }
    }
}
