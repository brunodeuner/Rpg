using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using RenewUp.Rpg.Dominio.Autenticação.Token;
using RenewUp.Rpg.Dominio.Dtos;
using RenewUp.Rpg.Dominio.RequisiçãoContexto;

namespace RenewUp.Rpg.Serviço.Website.Pwa.Autenticação
{
    public class EstadoDaAutenticação : AuthenticationStateProvider
    {
        private readonly IRequisiçãoContexto requisiçãoContexto;

        public EstadoDaAutenticação(IRequisiçãoContexto requisiçãoContexto) =>
            this.requisiçãoContexto = requisiçãoContexto;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (requisiçãoContexto?.Usuario?.Id is null)
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));

            var claim = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Nome") }, "Token");
            var claimPrincipal = new ClaimsPrincipal(claim);
            return Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public Task Deslogar()
        {
            requisiçãoContexto.Usuario = default;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public Task DefinirComoAutenticado(ClaimsPrincipal claimsPrincipal)
        {
            DefinirRequisiçãoContextoAPartirDoClaims(claimsPrincipal?.Identity as ClaimsIdentity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            Console.WriteLine(requisiçãoContexto.Usuario?.Id);
            return Task.CompletedTask;
        }

        private void DefinirRequisiçãoContextoAPartirDoClaims(ClaimsIdentity claimsIdentity)
        {
            requisiçãoContexto.Usuario = new UsuarioId(claimsIdentity?.Claims
                    .Where(claim => claim.Type.Equals(TokenClaims.UsuarioId(), StringComparison.OrdinalIgnoreCase))
                    .Select(claim => claim.Value)
                    .FirstOrDefault());
        }
    }
}
