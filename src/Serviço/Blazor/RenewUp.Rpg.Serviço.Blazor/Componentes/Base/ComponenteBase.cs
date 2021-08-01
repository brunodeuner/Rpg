using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base.Atributos;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Base
{
    public class ComponenteBase : ComponentBase
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            var propriedadesObrigatóriosNãoDefinidas = GetType()
                .GetProperties()
                .Where(x => ParâmetroObrigatórioENãoFoiInformado(x));
            if (propriedadesObrigatóriosNãoDefinidas.Any())
                throw new System.ArgumentException("Parâmetros obrigatórios não informados: " +
                    $"{string.Join(", ", propriedadesObrigatóriosNãoDefinidas.Select(x => x.Name))}");
        }

        private bool ParâmetroObrigatórioENãoFoiInformado(PropertyInfo propriedade) =>
            propriedade.GetCustomAttribute<ParâmetroObrigatórioAttribute>() is not null &&
            propriedade.GetValue(this) is null;
    }
}
