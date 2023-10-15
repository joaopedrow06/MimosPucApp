using WebUi.Pages.Modals;

namespace WebUi.Pages;

public partial class ClientsHistory
{
    [Inject]
    private ClientsService ClientsService { get; set; } = default!;
    [Inject]
    private NavigationManager UriHelper { get; set; } = default!;
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    public ServiceResponse<List<ClientsPets>> Response { get; set; } = default!;
    public List<ClientsPets> Histories { get; set; } = default!;
    private IEnumerable<ClientsPets> pagedData = default!;
    private MudTable<ClientsPets> table = new();

    private int totalItems;
    private string searchString = default!;
    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
    private async Task<TableData<ClientsPets>> ServerReload(TableState state)
    {
        Response = await ClientsService.GetAllFuturesAttendances();
        IEnumerable<ClientsPets> data = Response.Data!;
        await Task.Delay(300);
        data = data.Where(element =>
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Id.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Client.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Pets.Select(q => q.Name).ToList().Contains(searchString))
                return true;
            return false;
        }).ToArray();
        totalItems = data.Count();
        switch (state.SortLabel)
        {
            case "clientName":
                data = data.OrderByDirection(state.SortDirection, o => o.Client.Name);
                break;
            case "petName":
                data = data.OrderByDirection(state.SortDirection, o => o.Pets.Select(z => z.Name));
                break;
        }

        pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<ClientsPets>() { TotalItems = totalItems, Items = pagedData };
    }
    private void Detalhes(ClientsPets client)
    {
        UriHelper.NavigateTo($"ClientHistory/ClientDetails/{client.Client.Id}");
    }
    private void GoHome()
    {
        UriHelper.NavigateTo("/");
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
    private async Task NovoCliente()
    {
        var parameters = new DialogParameters<CadastrarClienteModal>();
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, NoHeader = true };
        var dialog = await DialogService.Show<CadastrarClienteModal>("Novo atendimento", parameters, options).Result;
    }

}
