using System;
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
        [Parameter]
        public Action Carregou { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await IndicadorDeCarregamento.Executar(cancellationToken =>
                    Inicializador.InicializarAsync(Context.Model, cancellationToken));
                Carregou?.Invoke();
            }
        }
    }
}
