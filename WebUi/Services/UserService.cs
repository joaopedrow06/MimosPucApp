using Models.Models;
using System.Net.Http;
using System.Net.Http.Json;
using WebUi.Pages;
using static System.Net.WebRequestMethods;

namespace WebUi.Services;

public class UsersService
{
    private readonly HttpClient _httpClient;
    public readonly string apiUri = string.Empty;

    public UsersService(HttpClient httpUser)
    {
        _httpClient = httpUser;
        apiUri = "https://localhost:7019/api/Users";
    }

    public async Task<ServiceResponse<List<Users>>> GetAll()
    {
        ServiceResponse<List<Users>>? response = new();
        try
        {
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Users>>>(apiUri);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<HttpResponseMessage> SaveUser(Users User)
    {
        return await _httpClient.PostAsJsonAsync(apiUri, User);
    }
    public async Task<HttpResponseMessage> EditUser(long UserId, Users User)
    {
        return await _httpClient.PutAsJsonAsync($"{apiUri}/{UserId}", User);
    }
    public async Task<HttpResponseMessage> DeleteUser(long id)
    {
        return await _httpClient.DeleteAsync($"{apiUri}/{id}");
    }
}
