using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento
{
    public partial class IndicadorDeCarregamento : ComponentBase, IDisposable
    {
        private CancellationTokenSource cancellationTokenSource;

        public async Task Executar(Func<CancellationToken, Task> tarefa)
        {
            cancellationTokenSource ??= new();
            Carregando = true;
            try
            {
                await tarefa(cancellationTokenSource.Token);
            }
            finally
            {
                Carregando = false;
            }
        }

        public bool Carregando { get; private set; }

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
