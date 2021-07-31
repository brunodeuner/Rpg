using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RenewUp.Rpg.Dominio.Autenticação.ComandosManipuladores;
using RenewUp.Rpg.Dominio.ContextoDaRequisição;
using RenewUp.Rpg.Dominio.Dtos;
using RenewUp.Rpg.Dominio.Repositorios;
using RenewUp.Rpg.Serviço.Website.Pwa.Autenticação;
using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Armazenamento.Gerenciador;
using SkyInfo.Infra.Armazenamento.Memoria;
using static RenewUp.Rpg.Serviço.Website.Pwa.Pages.Index;

namespace RenewUp.Rpg.Serviço.Website.Pwa
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("#app");

            InjetarContextoDaRequisição(builder.Services);
            InjetarServiçosDeAutenticação(builder.Services);
            builder.Services.AddMediatR(
                typeof(ManipularEventoDeUsuarioAutenticadoComSucesso),
                typeof(ManipuladorDoComandoDeAutenticação)
            );

            builder.Services.AddAuthorizationCore();

            builder.Services.AddSingleton(new DaoMemoria());
            builder.Services.AddSingleton<IDao>(serviceProvider =>
                new DaoGerenciador(serviceProvider, new ConfiguracaoGerenciador()
                {
                    Principal = new TipoGravacaoLeitura
                    {
                        Gravacao = typeof(DaoMemoria),
                        Leitura = typeof(DaoMemoria)
                    }
                }, default));
            builder.Services.AddSingleton<RepositorioBase<Teste>, RepositorioDeTeste>();

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
