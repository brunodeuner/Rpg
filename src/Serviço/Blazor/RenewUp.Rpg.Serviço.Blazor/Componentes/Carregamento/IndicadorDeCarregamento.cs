using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento
{
    public partial class IndicadorDeCarregamento : ComponentBase, IDisposable
    {
        private static readonly TimeSpan TempoParaAParecerOComponenteDeCarregando = TimeSpan.FromMilliseconds(150);
        private bool exibirCarregando;
        private ObservadorDeValor<bool> processando;
        private ObservadorDeValor<CancellationTokenSource> cancellationTokenSource;

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool Processando { get => processando.Valor; set => processando.Valor = value; }
        [Parameter]
        public EventCallback<bool> ProcessandoChanged
        {
            get => processando.ValorChanged; set => processando.ValorChanged = value;
        }
        [Parameter]
        public CancellationTokenSource CancellationTokenSource
        {
            get => cancellationTokenSource.Valor; set => cancellationTokenSource.Valor = value;
        }
        [Parameter]
        public EventCallback<CancellationTokenSource> CancellationTokenSourceChanged
        {
            get => cancellationTokenSource.ValorChanged; set => cancellationTokenSource.ValorChanged = value;
        }

        public async Task Executar(Func<CancellationToken, Task> tarefa)
        {
            Cancelar();
            CancellationTokenSource = new();
            var cancellationToken = CancellationTokenSource.Token;
            DefinirCarregando(true, cancellationToken);
            try
            {
                await tarefa(cancellationToken);
            }
            finally
            {
                DefinirCarregando(false, cancellationToken);
            }
        }

        private async void DefinirCarregando(bool valor, CancellationToken cancellationToken)
        {
            Processando = valor;
            if (valor)
                await Task.Delay(TempoParaAParecerOComponenteDeCarregando, cancellationToken);
            DefinirMonstrarCarregamento();
        }

        public void Cancelar()
        {
            if (CancellationTokenSource is null)
                return;

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }

        private void DefinirMonstrarCarregamento()
        {
            exibirCarregando = Processando;
            StateHasChanged();
        }

        public void Dispose()
        {
            Cancelar();
            GC.SuppressFinalize(this);
        }
    }
}
