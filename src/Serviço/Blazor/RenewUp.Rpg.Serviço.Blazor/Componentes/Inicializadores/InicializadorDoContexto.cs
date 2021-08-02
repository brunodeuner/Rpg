using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base.Atributos;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Carregamento;
using SkyInfo.Infra.Geral.Inicializador.Abstracoes;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Inicializadores
{
    public class InicializadorDoContexto : ComponenteBase
    {
        [Inject]
        public IInicializador Inicializador { get; set; }
        [CascadingParameter, ParâmetroObrigatório]
        public EditContext Context { get; set; }
        [Parameter, ParâmetroObrigatório]
        public IndicadorDeCarregamento IndicadorDeCarregamento { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await IndicadorDeCarregamento.Executar(cancellationToken =>
                Inicializador.InicializarAsync(Context.Model, cancellationToken));
        }
    }
}
