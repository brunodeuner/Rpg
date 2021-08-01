using Microsoft.Extensions.DependencyInjection;
using SkyInfo.Core.Dominio.Inicializador;
using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Armazenamento.Gerenciador;
using SkyInfo.Infra.Armazenamento.Memoria;
using SkyInfo.Infra.Bus.Abstracoes;
using SkyInfo.Infra.Dominio.Bus;
using SkyInfo.Infra.Geral.Inicializador.Abstracoes;

namespace RenewUp.Rpg.Aplicação
{
    public static class InjetorDeDependênciasDoServiço
    {
        public static void InjetarServiços(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IInicializador, Inicializador>();
            serviceCollection.AddSingleton<IMediatorHandler, MemoriaBus>();
            serviceCollection.AddSingleton(new DaoMemoria());
            serviceCollection.AddSingleton<IDao>(serviceProvider =>
                new DaoGerenciador(serviceProvider, new ConfiguracaoGerenciador()
                {
                    Principal = new TipoGravacaoLeitura
                    {
                        Gravacao = typeof(DaoMemoria),
                        Leitura = typeof(DaoMemoria)
                    }
                }, default));
        }
    }
}
