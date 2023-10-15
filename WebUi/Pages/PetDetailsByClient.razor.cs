using System.Runtime.CompilerServices;
using WebUi.Pages.Modals;

namespace WebUi.Pages;

public partial class PetDetailsByClient
{
    [Parameter]
    public string ClientId { get; set; } = string.Empty;
    public long clientId { get; set; } = default!;
    [Inject]
    private ClientsService ClientsService { get; set; } = default!;
    [Inject]
    private PetsService PetsService { get; set; } = default!;
    [Inject]
    private NavigationManager UriHelper { get; set; } = default!;
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    public ServiceResponse<ClientsPets> Response { get; set; } = default!;
    public List<Pets> Histories { get; set; } = default!;
    private IEnumerable<Pets> pagedData = default!;
    private MudTable<Pets> table = new();
    public string ClientName = string.Empty;
    private int totalItems;
    private string searchString = default!;
    private string PetName = default!;
    MudMessageBox Mbox { get; set; } = new();
    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
    private async Task<TableData<Pets>> ServerReload(TableState state)
    {
        clientId = Convert.ToInt64(ClientId);
        Response = await ClientsService.GetInfosByClientId(clientId);
        ClientName = Response.Data.Client.Name;
        IEnumerable<Pets> data = Response.Data.Pets;
        await Task.Delay(300);
        totalItems = data.Count();
        return new TableData<Pets>() { TotalItems = 1, Items = data };
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
    private async Task Delete(long petId, string name)
    {
        PetName = name;
        bool? result = await Mbox.Show();
        if (result != null)
        {
            var response = await PetsService.DeletePet(petId);
            if (!response.IsSuccessStatusCode)
            {
                StateHasChanged();
                return;
            }
            await table.ReloadServerData();
        }
    }
    private async Task Update(long petId, string name)
    {
        var parameters = new DialogParameters<AtualizarPetModal>();
        parameters.Add("PetId", petId);
        parameters.Add("PetName", name);
        parameters.Add("ClientId", clientId);
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, NoHeader = true };
        var dialog = await DialogService.Show<AtualizarPetModal>("Atualizar", parameters, options).Result;
        if (!dialog.Cancelled)
        {
            await table.ReloadServerData();
        }
    }
    private async Task NovoPet()
    {
        var parameters = new DialogParameters<CadastrarPetModal>();
        parameters.Add("ClientId", clientId);
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, NoHeader = true };
        var dialog = await DialogService.Show<CadastrarPetModal>("Novo Pet", parameters, options).Result;
        if (!dialog.Canceled)
        {
            await table.ReloadServerData();
        }
    }

}