using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Bus.Abstracoes;
using SkyInfo.Infra.Geral.Inicializador.Abstracoes;

namespace SkyInfo.Core.Dominio.Inicializador
{
    public class Inicializador : IInicializador
    {
        private readonly IDao dao;
        private readonly IMediatorHandler bus;
        private readonly IMapper mapper;

        public Inicializador(IDao dao, IMediatorHandler bus, IMapper mapper)
        {
            this.dao = dao;
            this.bus = bus;
            this.mapper = mapper;
        }

        public Task InicializarAsync<T>(T objeto, CancellationToken cancellationToken = default) =>
            new InicializadorArmazenamentoId(dao, bus, mapper).InicializarAsync(objeto, cancellationToken);
    }
}
