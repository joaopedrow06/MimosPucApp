

namespace WebUi.Services;

public class AppointmentsService
{
    private readonly HttpClient _httpClient;
    public readonly string apiUri = string.Empty;

    public AppointmentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        apiUri = _httpClient.BaseAddress!.ToString() + "Appointments";
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
}
