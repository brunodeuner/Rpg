using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using RenewUp.Rpg.Dominio.Autenticação.Token;
using RenewUp.Rpg.Dominio.ContextoDaRequisição;
using RenewUp.Rpg.Dominio.Dtos;

namespace RenewUp.Rpg.Serviço.Website.Pwa.Autenticação
{
    public sealed class EstadoDaAutenticação : AuthenticationStateProvider
    {
        private readonly IContextoDaRequisição contextoDaRequisição;

        public EstadoDaAutenticação(IContextoDaRequisição contextoDaRequisição) =>
            this.contextoDaRequisição = contextoDaRequisição;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (contextoDaRequisição?.Usuario?.Id is null)
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));

            var claim = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Nome") }, "Token");
            var claimPrincipal = new ClaimsPrincipal(claim);
            return Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public Task Deslogar()
        {
            contextoDaRequisição.LimparContexto();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public Task DefinirComoAutenticado(ClaimsPrincipal claimsPrincipal)
        {
            DefinirContextoDaRequisiçãoAPartirDoClaims(claimsPrincipal?.Identity as ClaimsIdentity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            Console.WriteLine(contextoDaRequisição.Usuario?.Id);
            return Task.CompletedTask;
        }

        private void DefinirContextoDaRequisiçãoAPartirDoClaims(ClaimsIdentity claimsIdentity) =>
            contextoDaRequisição.DefinirContexto(new UsuarioId(claimsIdentity?.Claims
                    .Where(claim => claim.Type.Equals(TokenClaims.UsuarioId(), StringComparison.OrdinalIgnoreCase))
                    .Select(claim => claim.Value)
                    .FirstOrDefault()));
    }
}
