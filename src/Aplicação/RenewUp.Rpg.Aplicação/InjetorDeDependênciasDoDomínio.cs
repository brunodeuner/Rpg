using Microsoft.Extensions.DependencyInjection;
using RenewUp.Rpg.Dominio.ContextoDaRequisição;
using RenewUp.Rpg.Dominio.Dtos;

namespace RenewUp.Rpg.Aplicação
{
    public static class InjetorDeDependênciasDoDomínio
    {
        public static void InjetarDomínio(this IServiceCollection serviceCollection) =>
            serviceCollection.InjetarContextoDaRequisição();

        private static void InjetarContextoDaRequisição(this IServiceCollection serviços)
        {
            var contextoDaRequisição = new ContextoDaRequisição();
            contextoDaRequisição.DefinirContexto(new UsuarioId("Usuário logado"));
            serviços.AddSingleton<IContextoDaRequisição>(contextoDaRequisição);
        }
    }
}
