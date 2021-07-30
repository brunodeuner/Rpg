using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RenewUp.Rpg.Dominio.Autenticação.ComandosManipuladores;
using RenewUp.Rpg.Serviço.Website.Pwa.Autenticação;
using SkyInfo.Core.Dominio.RequisicaoContexto;

namespace RenewUp.Rpg.Serviço.Website.Pwa
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<IRequisicaoContexto>(new RequisicaoContexto()
            {
                Usuario = new SkyInfo.Core.Dominio.Dtos.UsuarioId()
                {
                    Id = "Usuário logado"
                }
            });
            builder.Services.AddSingleton<EstadoDaAutenticação>();
            builder.Services.AddSingleton<AuthenticationStateProvider>(serviceProvider =>
                serviceProvider.GetRequiredService<EstadoDaAutenticação>());
            builder.Services.AddMediatR(
                typeof(ManipularEventoDeUsuarioAutenticadoComSucesso),
                typeof(ManipuladorDoComandoDeAutenticação)
            );

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
