namespace WebUi.Pages;

public partial class ClientsDetails
{
    [Parameter]
    public string ClientId { get; set; } = string.Empty;
    public long clientId { get; set; } = default!;
    [Inject]
    private ClientsService ClientsService { get; set; } = default!;
    [Inject]
    private NavigationManager UriHelper { get; set; } = default!;
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    public ServiceResponse<ClientsPets> Response { get; set; } = default!;
    public List<ClientsPets> Histories { get; set; } = default!;
    private IEnumerable<ClientsPets> pagedData = default!;
    private MudTable<ClientsPets> table = new();
    public string ClientName = string.Empty;
    private int totalItems;
    private string searchString = default!;
    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
    private async Task<TableData<ClientsPets>> ServerReload(TableState state)
    {
        clientId = Convert.ToInt64(ClientId);
        Response = await ClientsService.GetInfosByClientId(clientId);
        ClientName = Response.Data.Client.Name;
        var list = new List<ClientsPets>();
        list.Add(Response.Data);
        IEnumerable<ClientsPets> data = list;
        await Task.Delay(300);
        totalItems = data.Count();
        return new TableData<ClientsPets>() { TotalItems = 1, Items = data };
    }
    private void GoHome()
    {
        UriHelper.NavigateTo("/");
    }
    private void DetalhesPet(long clientId)
    {
        UriHelper.NavigateTo($"/ClientHistory/ClientDetails/PetDetailsByClient/{clientId}");
    }
    private void DetalhesFaturamento(long clientId)
    {
        UriHelper.NavigateTo($"/HistoryByClientId/{clientId}");
    }
    private void Submit()
    {
        UriHelper.NavigateTo("/Agendapetshop");
    }
    private void ClientHistory()
    {
        UriHelper.NavigateTo("/ClientHistory");
    }
    private void VetHistory()
    {
        UriHelper.NavigateTo("/Agendaveterinaria");
    }
    private void HistoricoPets()
    {
        UriHelper.NavigateTo("/HistoryPet");
    }
}