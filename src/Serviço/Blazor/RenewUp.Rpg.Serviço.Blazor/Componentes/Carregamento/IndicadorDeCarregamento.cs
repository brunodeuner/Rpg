using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento
{
    public partial class IndicadorDeCarregamento : ComponentBase, IDisposable
    {
        private static readonly TimeSpan TempoParaAParecerOComponenteDeCarregando = TimeSpan.FromMilliseconds(150);
        private CancellationTokenSource cancellationTokenSource;
        private bool exibirCarregando;

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public EventCallback<bool> AoAtualizarValorDeProcessando { get; set; }
        public bool Processando { get; private set; }

        public async Task Executar(Func<CancellationToken, Task> tarefa)
        {
            Cancelar();
            cancellationTokenSource ??= new();
            var cancellationToken = cancellationTokenSource.Token;
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
            if (Processando)
                await Task.Delay(TempoParaAParecerOComponenteDeCarregando, cancellationToken);
            await DefinirMonstrarCarregamento();
        }

        public void Cancelar()
        {
            if (cancellationTokenSource is null)
                return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        private async Task DefinirMonstrarCarregamento()
        {
            exibirCarregando = Processando;
            StateHasChanged();
            await AoAtualizarValorDeProcessando.InvokeAsync(Processando);
        }

        public void Dispose()
        {
            Cancelar();
            GC.SuppressFinalize(this);
        }
    }
}
