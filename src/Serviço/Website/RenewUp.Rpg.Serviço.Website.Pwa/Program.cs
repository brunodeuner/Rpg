using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RenewUp.Rpg.Aplicação;
using RenewUp.Rpg.Dominio.Autenticação.ComandosManipuladores;
using RenewUp.Rpg.Dominio.Repositorios;
using RenewUp.Rpg.Serviço.Website.Pwa.Autenticação;
using static RenewUp.Rpg.Serviço.Website.Pwa.Pages.Index;

namespace RenewUp.Rpg.Serviço.Website.Pwa
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.InjetarServiçosDeAutenticação();
            builder.Services.InjetarServiços();
            builder.Services.InjetarDomínio();

            builder.Services.AddSingleton<RepositorioBase<Teste>, RepositorioDeTeste>();
            builder.Services.AddMediatR(
               typeof(ManipularEventoDeUsuarioAutenticadoComSucesso),
               typeof(ManipuladorDoComandoDeAutenticação)
            );

            await builder.Build().RunAsync();
        }

        private static void InjetarServiçosDeAutenticação(this IServiceCollection serviços)
        {
            serviços.AddAuthorizationCore();
            serviços.AddSingleton<EstadoDaAutenticação>();
            serviços.AddSingleton<AuthenticationStateProvider>(serviceProvider =>
                serviceProvider.GetRequiredService<EstadoDaAutenticação>());
        }
    }
}
