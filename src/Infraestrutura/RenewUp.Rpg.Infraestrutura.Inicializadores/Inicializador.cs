using System.Threading;
using System.Threading.Tasks;
using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Bus.Abstracoes;
using SkyInfo.Infra.Geral.Inicializador.Abstracoes;

namespace SkyInfo.Core.Dominio.Inicializador
{
    public class Inicializador : IInicializador
    {
        private readonly IDao dao;
        private readonly IMediatorHandler bus;

        public Inicializador(IDao dao, IMediatorHandler bus)
        {
            this.dao = dao;
            this.bus = bus;
        }

        public Task InicializarAsync<T>(T objeto, CancellationToken cancellationToken = default) =>
            new InicializadorArmazenamentoId(dao, bus).InicializarAsync(objeto, cancellationToken);
    }
}
