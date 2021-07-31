using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RenewUp.Rpg.Dominio.Atributos;
using RenewUp.Rpg.Infraestrutura;

namespace RenewUp.Rpg.Serviço.CasosDeUso
{
    public static class Pesquisa
    {
        public static IQueryable<T> Pesquisar<T>(this IQueryable<T> query, string valorAPesquisar) =>
            query.FiltarEmTodasAsPropriedades(
                ObterPropriedadesParaFiltrar<T>().Select(x => x.Name), valorAPesquisar);

        private static IEnumerable<PropertyInfo> ObterPropriedadesParaFiltrar<T>() =>
            typeof(T).GetProperties().Where(x => x.GetCustomAttribute<FiltroAttribute>() is not null);
    }
}
