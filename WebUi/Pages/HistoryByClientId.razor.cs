using Models.Models;

namespace WebUi.Pages;

public partial class HistoryByClientId
{
    [Parameter]
    public string ClientId { get; set; }
    public long clientId { get; set; }
    [Inject]
    private AppointmentsService AppointmentsService { get; set; } = default!;
    [Inject]
    private NavigationManager UriHelper { get; set; } = default!;
    public ServiceResponse<List<TransactionHistories>> Response { get; set; } = default!;
    public List<TransactionHistories> Histories { get; set; } = default!;
    private IEnumerable<TransactionHistories> pagedData = default!;
    private MudTable<TransactionHistories> table = new();
    MudMessageBox Mbox { get; set; } = new();

    private int totalItems;
    private string searchString = default!;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<TransactionHistories>> ServerReload(TableState state)
    {
        clientId = Convert.ToInt64(ClientId);
        Response = await AppointmentsService.GetHistoryByClientId(clientId);
        IEnumerable<TransactionHistories> data = Response!.Data!;
        await Task.Delay(300);
        data = data!.Where(element =>
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Id.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Appointment.ClientPet.Client.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Appointment.ClientPet.Pet.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Appointment.AppointmentName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.TransactionDate.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }).ToArray();
        totalItems = data.Count();
        switch (state.SortLabel)
        {
            case "id":
                data = data.OrderByDirection(state.SortDirection, o => o.Id);
                break;
            case "clientName":
                data = data.OrderByDirection(state.SortDirection, o => o.Appointment.ClientPet.Client.Name);
                break;
            case "petName":
                data = data.OrderByDirection(state.SortDirection, o => o.Appointment.ClientPet.Pet.Name);
                break;
            case "appointment":
                data = data.OrderByDirection(state.SortDirection, o => o.Appointment.AppointmentName);
                break;
            case "date":
                data = data.OrderByDirection(state.SortDirection, o => o.TransactionDate);
                break;
        }

        pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<TransactionHistories>() { TotalItems = totalItems, Items = pagedData };
    }

    private async void AtendimentoCancelado(TransactionHistories transactionHistories)
    {
        bool? result = await Mbox.Show();
        if (result != null)
        {
            var response = await AppointmentsService.UpdateStatus(transactionHistories.AppointmentId);
            if (!response.Success)
            {
                StateHasChanged();
                return;
            }
            await table.ReloadServerData();
        }
    }

    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
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
}
