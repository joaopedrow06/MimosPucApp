using Newtonsoft.Json;
using System;

namespace WebUi.Pages.Modals
{
    public partial class AtualizarPetModal
    {
        [Inject] ISnackbar SnackbarService { get; set; } = default!;
        [Inject] PetsService PetsService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public long PetId { get; set; }
        [Parameter]
        public long ClientId { get; set; }
        [Parameter]
        public string PetName { get; set; } = default!;
        void Cancel() => MudDialog.Cancel();
        async void Submit(string NewName)
        {
            Pets pet = new Pets()
            {
                Id = PetId,
                Name = NewName,
                ClientId = ClientId,
            };
            var response = await PetsService.EditPet(PetId, pet);
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
