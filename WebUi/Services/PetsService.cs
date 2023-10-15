using Models.Models;
using System.Net.Http;
using System.Net.Http.Json;
using WebUi.Pages;
using static System.Net.WebRequestMethods;

namespace WebUi.Services;

public class PetsService
{
    private readonly HttpClient _httpClient;
    public readonly string apiUri = string.Empty;

    public PetsService(HttpClient httpPet)
    {
        _httpClient = httpPet;
        apiUri = "https://deploy-api-mimos.azurewebsites.net/api/Pets";
    }

    public async Task<ServiceResponse<List<Pets>>> GetAll()
    {
        ServiceResponse<List<Pets>>? response = new();
        try
        {
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Pets>>>(apiUri);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<HttpResponseMessage> SavePet(Pets pet)
    {
        return await _httpClient.PostAsJsonAsync(apiUri, pet);
    }
    public async Task<HttpResponseMessage> EditPet(long petId, Pets pet)
    {
        return await _httpClient.PutAsJsonAsync($"{apiUri}/{petId}", pet);
    }
    public async Task<HttpResponseMessage> DeletePet(long id)
    {
        return await _httpClient.DeleteAsync($"{apiUri}/{id}");
    }
}
