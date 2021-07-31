using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento
{
    public partial class IndicadorDeCarregamento : ComponentBase, IDisposable
    {
        private static readonly TimeSpan TempoParaAParecerOComponenteDeCarregando = TimeSpan.FromMilliseconds(150);

        private bool carregando;

        private CancellationTokenSource cancellationTokenSource;

        public bool MostrarCarregando { get; private set; }

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

        public bool Carregando { get => carregando; }

        private void DefinirCarregando(bool value, CancellationToken cancellationToken)
        {
            carregando = value;
            if (carregando)
                Task.Delay(TempoParaAParecerOComponenteDeCarregando, cancellationToken)
                    .ContinueWith((_) => MostrarCarregando = carregando);
            StateHasChanged();
        }

        public void Cancelar()
        {
            if (cancellationTokenSource is null)
                return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        public void Dispose()
        {
            Cancelar();
            GC.SuppressFinalize(this);
        }
    }
}
