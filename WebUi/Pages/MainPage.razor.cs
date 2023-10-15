using Newtonsoft.Json;
using WebUi.Pages.Modals;

namespace WebUi.Pages
{
    public partial class MainPage
    {
        [Inject]
        private AppointmentsService AppointmentsService { get; set; } = default!;
        [Inject]
        private NavigationManager UriHelper { get; set; } = default!;
        [Inject]
        private IDialogService DialogService { get; set; } = default!;
        public ServiceResponse<List<Appointments>> Response { get; set; } = default!;
        public List<Appointments> Appointments { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            Response = await AppointmentsService.GetAllFuturesAttendances();
            if (Response is not null)
            {
                Appointments = Response.Data.OrderBy(q => q.Date).Take(3).ToList()!;
            }
            StateHasChanged();
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
                    Appointments = Response.Data.OrderBy(q => q.Date).Take(3).ToList()!;
                }
                StateHasChanged();
            }
        }

    }
}
