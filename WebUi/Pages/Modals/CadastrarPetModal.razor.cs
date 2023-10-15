﻿using Newtonsoft.Json;
using System;

namespace WebUi.Pages.Modals
{
    public partial class CadastrarPetModal
    {
        [Inject] ISnackbar SnackbarService { get; set; } = default!;
        [Inject] PetsService PetsService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public long ClientId { get; set; }
        public string PetName { get; set; } = default!;
        void Cancel() => MudDialog.Cancel();
        async void Submit(string NewName)
        {
            Pets pet = new Pets()
            {
                Name = NewName,
                ClientId = ClientId,
            };
            var response = await PetsService.SavePet(pet);
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
