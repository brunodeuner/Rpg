﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Dominio.Repositorios;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento;
using RenewUp.Rpg.Serviço.CasosDeUso;
using SkyInfo.Infra.Armazenamento.Abstracoes.Avancado.Assincrono.Queryable;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Listagem
{
    public partial class Listagem<T> : ComponentBase
        where T : class, IId, new()
    {
        private string pesquisa;

        [Parameter]
        public IndicadorDeCarregamento IndicadorDeCarregamento { get; set; }

        public List<T> Registros { get; private set; } = new();
        public string Pesquisa
        {
            get => pesquisa; set
            {
                pesquisa = value;
                Pesquisar();
            }
        }

        [Inject]
        public RepositorioBase<T> Repositorio { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ObterRegistrosEAdicionarAListaMostrandoOCarregando();
        }

        public async void Pesquisar() => await ObterRegistrosEAdicionarAListaMostrandoOCarregando();

        private async Task ObterRegistrosEAdicionarAListaMostrandoOCarregando()
        {
            await IndicadorDeCarregamento.Executar(async cancellationToken =>
                await ObterRegistrosEAdicionarALista(cancellationToken));
        }

        private async Task ObterRegistrosEAdicionarALista(CancellationToken cancellationToken)
        {
            Registros.Clear();
            await foreach (var registro in ObterRegistros(cancellationToken))
            {
                Registros.Add(registro);
                StateHasChanged();
            }
        }

        public IAsyncEnumerable<T> ObterRegistros(CancellationToken cancellationToken) =>
            Repositorio.Selecionar().Pesquisar(pesquisa).ToAsyncEnumerable(cancellationToken);
    }
}
