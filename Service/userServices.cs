
using System.Net;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using SunPro_Jobs.Models;


namespace SunPro_Jobs.Service
{
    public class userServices
    {
        private readonly HttpClient _httpClient;

        private IJSRuntime _jsruntime;

        private const string BaseUrl = "https://localhost:7068";
        public userServices(HttpClient httpClient, IJSRuntime jsruntime)
        {
            _httpClient = httpClient;
            _jsruntime = jsruntime;
        }

        public async Task<bool> RegisterUser(UserModel userModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", userModel);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                    if (responseData != null)
                    {
                        await _jsruntime.InvokeVoidAsync("localStorage.setItem", "UserId", responseData.UserId.ToString());
                        await _jsruntime.InvokeVoidAsync("localStorage.setItem", "Names", responseData.Names);
                        Console.WriteLine($"User registered successfully with UserId: {responseData.UserId}");
                        return true;
                    }

                }
                Console.WriteLine("Failed to register user.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> LoginUser(LoginModel loginModel)
        {
            var user = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginModel);
            if (user.IsSuccessStatusCode)
            {
                var responseContent = await user.Content.ReadFromJsonAsync<LoginResponse>();
                if (responseContent?.Token != null)
                {
                    await _jsruntime.InvokeVoidAsync("localStorage.setItem", "authToken", responseContent.Token);
                    await _jsruntime.InvokeVoidAsync("localStorage.setItem", "UserId", responseContent.UserId.ToString());
                    await _jsruntime.InvokeVoidAsync("localStorage.setItem", "role", responseContent.Role);
                   
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
            await _jsruntime.InvokeVoidAsync("localStorage.removeItem", "role");
        }
        public async Task<string?> GetUserName()
        {
            return await _jsruntime.InvokeAsync<string?>("localStorage.getItem", "Names");
        }

        public async Task<bool> UpdateUser(UserDetailDto userDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/update", userDto);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine("User updated successfully (204 No Content).");
                    return true;
                }
                else if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to update user. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }
            return false;
        }


        public async Task<UserDetailDto?> GetUserById(int userId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserDetailDto>($"{BaseUrl}/api/User/{userId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID: {ex.Message}");
                return null;
            }
        }


        public async Task<string?> GetUserId()
        {
            return await _jsruntime.InvokeAsync<string?>("localStorage.getItem", "UserId");
        }


        public async Task<string?> GetRole()
        {
            var role = await _jsruntime.InvokeAsync<string?>("localStorage.getItem", "Role");
            Console.WriteLine($"Role from localStorage: {role}");
            return role;
        }

        public async Task<List<UserDetailDto>> GetAllUsersAsync()
        {
            try
            {
               
                var response = await _httpClient.GetFromJsonAsync<List<UserDetailDto>>($"{BaseUrl}/api/User/GetAllUsers");

                return response ?? new List<UserDetailDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }





        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/api/User/Delete/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"User with ID {userId} deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to delete user with ID {userId}. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetCurrentUserIdAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/User/GetCurrentUserId");
            if (response.IsSuccessStatusCode)
            {
                var userId = await response.Content.ReadAsStringAsync();
                return int.TryParse(userId, out var id) ? id : 0;
            }

            return 0;
        }
    }
}
