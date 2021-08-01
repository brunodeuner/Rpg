using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Armazenamento.Abstracoes.Generic;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;
using SkyInfo.Infra.Bus.Abstracoes;
using SkyInfo.Infra.Dominio.Notificacao;
using SkyInfo.Infra.Geral.Inicializador.Abstracoes;
using SkyInfo.Infra.Geral.ObjetoInformacao;

namespace SkyInfo.Core.Dominio.Inicializador
{
    public class InicializadorArmazenamentoId : IInicializador
    {
        private readonly IDao dao;
        private readonly IMediatorHandler bus;
        private readonly IMapper mapper;

        public InicializadorArmazenamentoId(IDao dao, IMediatorHandler bus, IMapper mapper)
        {
            this.dao = dao;
            this.bus = bus;
            this.mapper = mapper;
        }
        public async Task InicializarAsync<T>(T objeto, CancellationToken cancellationToken = default)
        {
            foreach (var informacoesInicializador in ObterInformacoesInicializador(objeto))
                await Inicializar(informacoesInicializador, cancellationToken);
        }

        private async Task Inicializar(InformacoesInicializador informacoesInicializador,
            CancellationToken cancellationToken)
        {
            var leituras = await ObterLeituras(informacoesInicializador, cancellationToken);
            if (leituras.Any(x => x == default))
            {
                await bus.PublicarEvento(new NotificacaoDominio(new ObjetoInformacao
                {
                    Descricao = "Referência inexistente"
                }), cancellationToken);
                return;
            }

            if (informacoesInicializador.EhLista)
                AtribuirValorAPropriedadeDoTipoLista(informacoesInicializador, leituras);
            else
                DefinirValor(informacoesInicializador, leituras.First());
        }

        private void DefinirValor(InformacoesInicializador informacoesInicializador, object valor)
        {
            if (informacoesInicializador.Propriedade is not null)
                informacoesInicializador.Propriedade.SetValue(informacoesInicializador.ObjetoPropriedade, valor);
            else
                mapper.Map(valor, informacoesInicializador.ObjetoPropriedade,
                    valor.GetType(), informacoesInicializador.ObjetoPropriedade.GetType());
        }

        private void AtribuirValorAPropriedadeDoTipoLista(InformacoesInicializador informacoesInicializador,
            List<object> leituras)
        {
            var tipoGenerico = informacoesInicializador.Propriedade.PropertyType.GetGenericArguments().First();
            var novaLista = Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoGenerico));
            foreach (var leitura in leituras)
                novaLista.GetType().GetMethod(nameof(ICollection<object>.Add)).Invoke(novaLista,
                    new[] { leitura });
            DefinirValor(informacoesInicializador, novaLista);
        }

        private async Task<List<object>> ObterLeituras(InformacoesInicializador informacoesInicializador,
            CancellationToken cancellationToken)
        {
            var leituras = new List<object>();
            foreach (var id in informacoesInicializador.Ids)
            {
                if (id.ChavePreenchida())
                {
                    var method = typeof(ILeituraGenericAsync).GetMethods().FirstOrDefault(
                        x => x.Name == nameof(ISelecionarAsync.SelecionarAsync));
                    var genericMethod = method.MakeGenericMethod(id.GetType());
                    var task = genericMethod.Invoke(dao, new object[] { id, cancellationToken });
                    leituras.Add(await (dynamic)task);
                }
                else
                {
                    leituras.Add(default);
                }
            }

            return leituras;
        }

        private List<InformacoesInicializador> ObterInformacoesInicializador<T>(T objeto)
        {
            var listaInformacoesInicializador = new List<InformacoesInicializador>();
            if (Equals(objeto, default(T)))
                return listaInformacoesInicializador;

            ProcessarPropriedade(objeto, ref listaInformacoesInicializador, default, objeto);

            foreach (var propriedade in objeto.GetType().GetProperties()
                .Where(x => x.CanRead && x.CanWrite && !x.GetIndexParameters().Any()))
            {
                var valorPropriedade = propriedade.GetValue(objeto);
                ProcessarPropriedade(objeto, ref listaInformacoesInicializador, propriedade, valorPropriedade);
            }
            return listaInformacoesInicializador;
        }

        private void ProcessarPropriedade<T>(T objeto,
            ref List<InformacoesInicializador> listaInformacoesInicializador, PropertyInfo propriedade,
            object valorPropriedade)
        {
            if (valorPropriedade is IId id)
            {
                ProcessarEntidade(objeto, ref listaInformacoesInicializador, propriedade, id);
                return;
            }
            if (valorPropriedade is IEnumerable<dynamic> listaValorPropriedade)
            {
                ProcessarLista(objeto, ref listaInformacoesInicializador, propriedade, listaValorPropriedade);
                return;
            }
            if (propriedade?.PropertyType.IsClass ?? false)
                ProcessarClasse(listaInformacoesInicializador, valorPropriedade);
        }

        private static void ProcessarEntidade<T>(T objeto,
            ref List<InformacoesInicializador> listaInformacoesInicializador, PropertyInfo propriedade, IId id)
        {
            listaInformacoesInicializador.Add(new InformacoesInicializador(new List<IId> { id },
                propriedade, objeto, false));
        }

        private void ProcessarClasse(List<InformacoesInicializador> listaInformacoesInicializador,
            object valorPropriedade)
        {
            var listaInformacoesInicializadorObjeto = ObterInformacoesInicializador(valorPropriedade);
            if (listaInformacoesInicializadorObjeto.Any())
                listaInformacoesInicializador.AddRange(listaInformacoesInicializadorObjeto);
        }

        private void ProcessarLista<T>(T objeto, ref List<InformacoesInicializador> listaInformacoesInicializador,
            PropertyInfo propriedade, IEnumerable<dynamic> listaValorPropriedade)
        {
            if (!listaValorPropriedade.Any())
                return;
            if (listaValorPropriedade.First() is IId)
            {
                listaInformacoesInicializador.Add(new InformacoesInicializador(
                    listaValorPropriedade.Cast<IId>(), propriedade, objeto, true));
                return;
            }
            ObterInformacoesDosRegistrosDaLista(ref listaInformacoesInicializador, listaValorPropriedade);
        }

        private void ObterInformacoesDosRegistrosDaLista(
            ref List<InformacoesInicializador> listaInformacoesInicializador,
            IEnumerable<dynamic> listaValorPropriedade)
        {
            foreach (var valorItemPropriedade in listaValorPropriedade)
            {
                var listaInformacoesInicializadorItemLista = ObterInformacoesInicializador(
                    valorItemPropriedade) as List<InformacoesInicializador>;
                if (listaInformacoesInicializadorItemLista.Any())
                    listaInformacoesInicializador.AddRange(listaInformacoesInicializadorItemLista);
            }
        }
    }

    public struct InformacoesInicializador
    {
        public InformacoesInicializador(IEnumerable<IId> ids, PropertyInfo propriedade, object objetoPropriedade,
            bool ehLista)
        {
            Ids = ids;
            Propriedade = propriedade;
            ObjetoPropriedade = objetoPropriedade;
            EhLista = ehLista;
        }

        public IEnumerable<IId> Ids { get; private set; }
        public PropertyInfo Propriedade { get; private set; }
        public object ObjetoPropriedade { get; private set; }
        public bool EhLista { get; private set; }
    }
}
