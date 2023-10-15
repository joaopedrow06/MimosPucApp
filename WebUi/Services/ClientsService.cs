using System.Net.Http;
using System.Net.Http.Json;
using WebUi.Pages;

namespace WebUi.Services;

public class ClientsService
{
    private readonly HttpClient _httpClient;
    public readonly string apiUri = string.Empty;

    public ClientsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        apiUri = "https://deploy-api-mimos.azurewebsites.net/api/Clients";
    }

    public async Task<ServiceResponse<List<Clients>>> GetAll()
    {
        ServiceResponse<List<Clients>>? response = new();
        try
        {
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Clients>>>(apiUri);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<List<ClientsPets>>> GetAllFuturesAttendances()
    {
        ServiceResponse<List<ClientsPets>>? response = new();
        try
        {
            var route = apiUri + "/GetClientsPetsGroupedByClientId";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<ClientsPets>>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<ClientsPets>> GetInfosByClientId(long ClientId)
    {
        ServiceResponse<ClientsPets>? response = new();
        try
        {
            var route = $"{apiUri}/GetInfosByClientId/{ClientId}";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<ClientsPets>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<ClientsPets>> GetByClientPetId(long ClientId, long PetId)
    {
        ServiceResponse<ClientsPets>? response = new();
        try
        {
            var route = $"{apiUri}/GetClientPetByClientAndPetId/{ClientId}/{PetId}";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<ClientsPets>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<HttpResponseMessage> Save(Clients entity)
    {
        return await _httpClient.PostAsJsonAsync(apiUri, entity);
    }
}
