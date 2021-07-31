using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SkyInfo.Infra.Armazenamento.Abstracoes.Avancado.Assincrono.Queryable;

namespace RenewUp.Rpg.Infraestrutura
{
    public static class Filtrar
    {
        private static readonly MethodInfo Contains = typeof(string)
            .GetMethod(nameof(string.Contains), new[] { typeof(string) });

        public static IQueryable<T> FiltarEmTodasAsPropriedades<T>(
            this IQueryable<T> query, IEnumerable<string> propriedades, string valorAFiltar)
        {
            if (valorAFiltar.Preenchido())
            {
                var queryableDao = query as IQueryableDao<T>;
                var éQueryableDao = queryableDao is not null;
                if (éQueryableDao)
                    query = queryableDao.QueryOriginal;

                query = query.PesquisarEmTodosAsPropriedadesInterno(propriedades, valorAFiltar);

                if (éQueryableDao)
                    query = new QueryableDao<T>(query, new QueryProviderDao(query.Provider,
                        (queryableDao.Provider as IQueryProviderDao).Dao)).AsQueryable();
            }

            return query;
        }

        private static IQueryable<T> PesquisarEmTodosAsPropriedadesInterno<T>(
            this IQueryable<T> query, IEnumerable<string> propriedades, string valorAFiltar)
        {
            var parametro = Expression.Parameter(typeof(T));
            var expressãoDoValorAPesquisar = Expression.Constant(valorAFiltar);
            var expressõesDeContains = propriedades
                .Select(x => Expression.Call(
                        Expression.Property(parametro, x), Contains, expressãoDoValorAPesquisar))
                .Cast<Expression>();
            var expressõesDeContainsComCondiçãoOu = expressõesDeContains
                .Aggregate((a, b) => Expression.OrElse(a, b));
            var expressãoWhere = Expression.Lambda<Func<T, bool>>(expressõesDeContainsComCondiçãoOu, parametro);

            return query.Where(expressãoWhere).AsQueryable();
        }
    }
}
