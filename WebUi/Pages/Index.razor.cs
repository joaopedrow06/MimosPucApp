using Microsoft.AspNetCore.Components;
using Models.Models;
using WebUi.Pages.Modals;

namespace WebUi.Pages;

public partial class Index
{
    [Inject] UsersService UsersService { get; set; } = default!;
    [Inject] private NavigationManager UriHelper { get; set; } = default!;
    [Inject] ISnackbar SnackbarService { get; set; } = default!;
    public string username { get; set; } = "";
    public string password { get; set; } = "";
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    bool isShow;

    public async void ValidateLogin()
    {
        Users attemptLogin = new Users()
        {
            Name = username,
            Password = password
        };
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
    void ButtonTestclick()
    {
        if(isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    private void GoHome()
    {
        UriHelper.NavigateTo("/home");
    }

}
