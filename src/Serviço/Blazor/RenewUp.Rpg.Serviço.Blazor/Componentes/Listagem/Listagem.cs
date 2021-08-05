using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Serviço.CasosDeUso;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Listagem
{
    public partial class Listagem<T> : ComponentBase
        where T : class, IId, new()
    {
        private string pesquisa;
        private RegistrosDaListagem<T> registrosDaListagem;

        public string Pesquisa
        {
            get => pesquisa; set
            {
                pesquisa = value;
                Pesquisar();
            }
        }

        public async void Pesquisar() => await registrosDaListagem
            .ObterRegistrosEAdicionarAListaMostrandoOCarregando();

        private ValueTask<IQueryable<T>> Filtrar(IQueryable<T> query, CancellationToken cancellationToken) =>
            ValueTask.FromResult(query.Pesquisar(Pesquisa));
    }
}