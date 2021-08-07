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
        private CancellationTokenSource cancellationTokenSource;
        private bool exibirCarregando;
        private ObservadorDeValor<bool> processando;

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool Processando { get => processando.Valor; set => processando.Valor = value; }
        [Parameter]
        public EventCallback<bool> ProcessandoChanged { get => processando.ValorChanged; set => processando.ValorChanged = value; }

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
            if (valor)
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
