using ASP_Homework;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lesson1
{
    public class ApiService(HttpClient httpClient, IConfiguration config)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _reqResBaseUrl = config["ApiSettings:ReqResBaseUrl"]!;
        private readonly string _jsonPlaceholderBaseUrl = config["ApiSettings:JsonPlaceholderBaseUrl"]!;

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_reqResBaseUrl}/users/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<SingleUserResponse>();
            return result?.Data;
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_reqResBaseUrl}/users", user);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<User>() : null;
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_reqResBaseUrl}/users/{id}", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_reqResBaseUrl}/users/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAndTitleAsync(int userId, string title)
        {
            var url = $"{_jsonPlaceholderBaseUrl}/posts?userId={userId}&title={Uri.EscapeDataString(title)}";
            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<List<Post>>() ?? [] : [];
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_jsonPlaceholderBaseUrl}/posts/{id}");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Post>() : null;
        }

        public async Task<Post?> CreatePostAsync(Post post)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_jsonPlaceholderBaseUrl}/posts", post);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Post>() : null;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_jsonPlaceholderBaseUrl}/posts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}