using Newtonsoft.Json;
using System;

namespace WebUi.Pages.Modals
{
    public partial class CadastrarClienteModal
    {
        [Inject] ISnackbar SnackbarService { get; set; } = default!;
        [Inject] ClientsService ClientsService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        public string ClientName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string CellPhone { get; set; } = default!;
        public string CEP { get; set; } = default!;
        public string Adress { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        void Cancel() => MudDialog.Cancel();
        async void Submit(string NewName)
        {
            Clients client = new Clients()
            {
                Name = NewName,
                Email = Email,
                CellPhone = CellPhone,
                Adress = Adress,
                CEP = CEP,
                HouseNumber = HouseNumber,
            };
            var response = await ClientsService.Save(client);
            var message =  response.Content.ReadAsStringAsync().Result;
            if(response.IsSuccessStatusCode)
            {
                dynamic result = JsonConvert.DeserializeObject<dynamic>(message);
                message = result!.message.Value;
            }
            SnackbarService.Add<MudChip>(new Dictionary<string, object>() {
                { "Text", message },
                { "Color", Color.Primary }
                });
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
