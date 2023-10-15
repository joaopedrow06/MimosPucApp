using System.Net.Http;
using System.Net.Http.Json;
using WebUi.Pages;

namespace WebUi.Services;

public class AppointmentsService
{
    private readonly HttpClient _httpClient;
    public readonly string apiUri = string.Empty;

    public AppointmentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        apiUri = "https://deploy-api-mimos.azurewebsites.net/api/Appointments";
    }

    public async Task<ServiceResponse<List<Appointments>>> GetAll()
    {
        ServiceResponse<List<Appointments>>? response = new();
        try
        {
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Appointments>>>(apiUri);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<List<Appointments>>> GetAllFuturesAttendances()
    {
        ServiceResponse<List<Appointments>>? response = new();
        try
        {
            var route = apiUri + "/GetAllFutures";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Appointments>>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<List<TransactionHistories>>> GetHistoryPetShop()
    {
        ServiceResponse<List<TransactionHistories>>? response = new();
        try
        {
            var route = apiUri + "/GetHistoryPetShop";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<TransactionHistories>>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<HttpResponseMessage> SavePet(Appointments appoiment)
    {
        return await _httpClient.PostAsJsonAsync(apiUri, appoiment);
    }
    public async Task<ServiceResponse<List<TransactionHistories>>> GetHistoryByClientId(long ClientId)
    {
        ServiceResponse<List<TransactionHistories>>? response = new();
        try
        {
            var route = $"{apiUri}/GetHistoryByClientId/{ClientId}";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<TransactionHistories>>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
    public async Task<ServiceResponse<List<Appointments>>> UpdateStatus(long AppointmentId)
    {
        ServiceResponse<List<Appointments>>? response = new();
        try
        {
            var route = $"{apiUri}/AppointmentCanceled/{AppointmentId}";
            response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Appointments>>>(route);
        }
        catch (Exception ex)
        {
            response!.Success = false;
            response!.Message = ex.Message;
        }
        return response!;
    }
}
