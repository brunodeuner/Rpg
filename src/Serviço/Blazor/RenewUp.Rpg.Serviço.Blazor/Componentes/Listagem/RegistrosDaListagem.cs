using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Dominio.Repositorios;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;
using SkyInfo.Infra.Armazenamento.Abstracoes.Avancado.Assincrono.Queryable;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Listagem
{
    public partial class RegistrosDaListagem<T> : ComponenteBase
        where T : class, IId, new()
    {
        private IndicadorDeCarregamento indicadorDeCarregamento;

        [Inject]
        public RepositorioBase<T> Repositorio { get; set; }
        [Parameter]
        public Func<IQueryable<T>, CancellationToken, ValueTask<IQueryable<T>>> OnQuery { get; set; }
        public List<T> Registros { get; private set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                await ObterRegistrosEAdicionarAListaMostrandoOCarregando();
        }

        public Task ObterRegistrosEAdicionarAListaMostrandoOCarregando() =>
            indicadorDeCarregamento.Executar(async cancellationToken =>
                await ObterRegistrosEAdicionarALista(cancellationToken));

        private async Task ObterRegistrosEAdicionarALista(CancellationToken cancellationToken)
        {
            Registros.Clear();
            await foreach (var registro in ObterRegistros(cancellationToken))
            {
                Registros.Add(registro);
                StateHasChanged();
            }
        }

        private async IAsyncEnumerable<T> ObterRegistros([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Repositorio.Selecionar();
            if (OnQuery is not null)
                query = await OnQuery(query, cancellationToken);
            await foreach (var resultado in query.ToAsyncEnumerable(cancellationToken))
                yield return resultado;
        }
    }
}
