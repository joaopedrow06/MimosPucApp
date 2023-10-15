using Models.Enums;
using Models.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace WebUi.Pages.Modals
{
    public partial class NovoAgendamento
    {
        [Inject] ISnackbar SnackbarService { get; set; } = default!;
        [Inject] PetsService PetsService { get; set; } = default!;
        [Inject] ClientsService ClientsService { get; set; } = default!;
        [Inject] AppointmentsService AppointmentsService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
        public List<Clients> Clients { get; set; } = default!;
        public List<Pets> Pets { get; set; } = default!;
        public Clients Client { get; set; } = default!;
        public Pets Pet { get; set; } = default!;
        public Appointments Appointment { get; set; } = default!;
        public Enum AppointmentType { get; set; } = default!;
        public DateTime Date = DateTime.Today;
        public TimeSpan Time { get; set; } = TimeSpan.Zero;
        protected override async Task OnInitializedAsync()
        {
            var ResponseClients = await ClientsService.GetAll();
            if(ResponseClients.Success)
            {
                Clients = ResponseClients.Data;
            }
            StateHasChanged();
        }
        void Cancel() => MudDialog.Cancel();
        async void Submit()
        {
            var ClientPetResponse = await ClientsService.GetByClientPetId(Client.Id,Pet.Id);
            Appointments appointment = new Appointments()
            {
                AppointmentIsComplete = false,
                AppointmentName = (AppointmentNames)AppointmentType,
                WasCanceled = false,
                Date = Date.Date.AddMinutes(Time.TotalMinutes),
                ClientPetId = ClientPetResponse.Data.Id,
                ClientPet = ClientPetResponse.Data
            };
            var response = await AppointmentsService.SavePet(appointment);
            var message = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic result = JsonConvert.DeserializeObject<dynamic>(message);
                message = result!.message.Value;
            }
            SnackbarService.Add<MudChip>(new Dictionary<string, object>() {
                { "Text", message },
                { "Color", Color.Primary }
                });
        }
        private async Task HandleClientChanged(Clients client)
        {
            if (client == null)
            {
                Client = new();
                Pet = new();
                Pets = new();
            }
            else
            {
                Client = client;
                var responsePets = await ClientsService.GetInfosByClientId(client.Id);
                if(responsePets.Success)
                {
                    Pets = responsePets.Data.Pets;
                }
            }

        }
        private async Task HandlePetChanged(Pets pet)
        {
            if (pet == null)
            {
                Pet = new();
            }
            else
            {
                Pet = pet;
            }

        }
        private async Task SetAppointment(Enum appointmentSelected)
        {
            AppointmentType = appointmentSelected;
        }
        public void HandleDayChange(DateTime? date)
        {
            Date = date.Value;
        }
        public void HandleHourChange(TimeSpan? date)
        {
            Time = date.Value;
        }
    }
}
