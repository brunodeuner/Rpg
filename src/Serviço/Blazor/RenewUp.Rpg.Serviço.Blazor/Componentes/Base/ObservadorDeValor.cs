using Microsoft.AspNetCore.Components;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Base
{
    public struct ObservadorDeValor<T>
    {
        private T valor;

        public T Valor
        {
            get => valor;
            set
            {
                var valorAnterior = valor;
                valor = value;

                if (valor is null && valorAnterior is not null)
                {
                    NotificarAoAlterarValor();
                    return;
                }

                if (valor is not null && !valor.Equals(valorAnterior))
                    NotificarAoAlterarValor();
            }
        }

        public EventCallback<T> ValorChanged { get; set; }

        private async void NotificarAoAlterarValor()
        {
            if (ValorChanged.HasDelegate)
                await ValorChanged.InvokeAsync(valor);
        }
    }
}
