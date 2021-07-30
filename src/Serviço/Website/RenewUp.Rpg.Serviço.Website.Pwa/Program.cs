using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RenewUp.Rpg.Dominio.Autenticação.ComandosManipuladores;
using RenewUp.Rpg.Dominio.ContextoDaRequisição;
using RenewUp.Rpg.Dominio.Dtos;
using RenewUp.Rpg.Serviço.Website.Pwa.Autenticação;

namespace RenewUp.Rpg.Serviço.Website.Pwa
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            InjetarContextoDaRequisição(builder.Services);
            InjetarServiçosDeAutenticação(builder.Services);
            builder.Services.AddMediatR(
                typeof(ManipularEventoDeUsuarioAutenticadoComSucesso),
                typeof(ManipuladorDoComandoDeAutenticação)
            );

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }

        private static void InjetarContextoDaRequisição(IServiceCollection serviços)
        {
            var contextoDaRequisição = new ContextoDaRequisição();
            contextoDaRequisição.DefinirContexto(new UsuarioId("Usuário logado"));
            serviços.AddSingleton<IContextoDaRequisição>(contextoDaRequisição);
        }

        private static void InjetarServiçosDeAutenticação(IServiceCollection serviços)
        {
            serviços.AddSingleton<EstadoDaAutenticação>();
            serviços.AddSingleton<AuthenticationStateProvider>(serviceProvider =>
                serviceProvider.GetRequiredService<EstadoDaAutenticação>());
        }
    }
}
