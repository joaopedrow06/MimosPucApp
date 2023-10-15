using Newtonsoft.Json;
using WebUi.Shared;

namespace WebUi.Pages
{
    public partial class Login
    {
        [Inject] UsersService UsersService { get; set; } = default!;
        [Inject] private NavigationManager UriHelper { get; set; } = default!;
        [Inject] ISnackbar SnackbarService { get; set; } = default!;
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public async void ValidateLogin()
        {
            Users attemptLogin = new Users();
            var ValidLogin = await UsersService.ValidateUser(attemptLogin);
            var message = ValidLogin.Content.ReadAsStringAsync().Result;
            if (ValidLogin.IsSuccessStatusCode)
            {
                GoHome();
            }
            else
            {
                SnackbarService.Add<MudChip>(new Dictionary<string, object>() {
                { "Text", message },
                { "Color", Color.Primary }
                });
            }
        }
        private void GoHome()
        {
            UriHelper.NavigateTo("/home");
        }

    }
}
