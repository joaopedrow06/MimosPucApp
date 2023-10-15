using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace WebUi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<AppointmentsService,AppointmentsService>();
            builder.Services.AddScoped<ClientsService, ClientsService>();
            builder.Services.AddScoped<PetsService, PetsService>();
            builder.Services.AddMudServices();
            await builder.Build().RunAsync();

        }

    }
}