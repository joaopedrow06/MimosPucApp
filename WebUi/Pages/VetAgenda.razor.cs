using Microsoft.AspNetCore.Components;
using WebUi.Pages.Modals;

namespace WebUi.Pages;

public partial class VetAgenda
{
    [Inject]
    private AppointmentsService AppointmentsService { get; set; } = default!;
    [Inject]
    private NavigationManager UriHelper { get; set; } = default!;
    public ServiceResponse<List<Appointments>> Response { get; set; } = default!;
    public List<Appointments> Appointments1 { get; set; } = default!;
    public List<Appointments> Appointments2 { get; set; } = default!;
    public List<Appointments> Appointments3 { get; set; } = default!;
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
        Response = await AppointmentsService.GetAllFuturesAttendances();
        if (Response is not null)
        {
            Response.Data = Response.Data.Where(q => q.AppointmentName == Models.Enums.AppointmentNames.VET).ToList();
            Appointments1 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && q.Date >= DateTime.Now).ToList();
            Appointments2 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.AddDays(1).ToShortDateString()).ToList();
            Appointments3 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.AddDays(2).ToShortDateString()).ToList();
        }
        StateHasChanged();
    }
    private async Task NovoAgendamentoAtendimento()
    {
        var parameters = new DialogParameters<NovoAgendamento>();
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, NoHeader = true };
        var dialog = await DialogService.Show<NovoAgendamento>("Novo atendimento", parameters, options).Result;
        if (!dialog.Cancelled)
        {
            Response = await AppointmentsService.GetAllFuturesAttendances();
            if (Response is not null)
            {
                Response.Data = Response.Data.Where(q => q.AppointmentName == Models.Enums.AppointmentNames.VET).ToList();
                Appointments1 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && q.Date >= DateTime.Now).ToList();
                Appointments2 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.AddDays(1).ToShortDateString()).ToList();
                Appointments3 = Response.Data!.Where(q => q.Date.ToShortDateString() == DateTime.Now.AddDays(2).ToShortDateString()).ToList();
            }
            StateHasChanged();
        }
    }
    private void GoHome()
    {
        UriHelper.NavigateTo("/home");
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