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
        private bool carregando;
        private bool mostrarCarregando;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

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
            carregando = valor;
            if (carregando)
                await Task.Delay(TempoParaAParecerOComponenteDeCarregando, cancellationToken);
            DefinirMonstrarCarregamento();
        }

        public void Cancelar()
        {
            if (cancellationTokenSource is null)
                return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        private void DefinirMonstrarCarregamento()
        {
            mostrarCarregando = carregando;
            StateHasChanged();
        }

        public void Dispose()
        {
            Cancelar();
            GC.SuppressFinalize(this);
        }
    }
}
