using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using SkyInfo.Core.Dominio.Dtos;
using SkyInfo.Core.Dominio.RequisicaoContexto;
using SkyInfo.Core.Dominio.Token;

namespace RenewUp.Rpg.Serviço.Website.Pwa.Autenticação
{
    public class EstadoDaAutenticação : AuthenticationStateProvider
    {
        private readonly IRequisicaoContexto requisicaoContexto;

        public EstadoDaAutenticação(IRequisicaoContexto requisicaoContexto) =>
            this.requisicaoContexto = requisicaoContexto;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (requisicaoContexto?.Usuario?.Id is null)
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));

            var claim = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Nome") }, "Token");
            var claimPrincipal = new ClaimsPrincipal(claim);
            return Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public Task Deslogar()
        {
            requisicaoContexto.Usuario = default;
            requisicaoContexto.Organizacao = default;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public Task DefinirComoAutenticado(ClaimsPrincipal claimsPrincipal)
        {
            DefinirRequisiçãoContextoAPartirDoClaims(claimsPrincipal?.Identity as ClaimsIdentity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            Console.WriteLine(requisicaoContexto.Usuario?.Id);
            return Task.CompletedTask;
        }

        private void DefinirRequisiçãoContextoAPartirDoClaims(ClaimsIdentity claimsIdentity)
        {
            requisicaoContexto.Usuario = new UsuarioId(claimsIdentity?.Claims
                    .Where(claim => claim.Type.Equals(TokenClaims.UsuarioId(), StringComparison.OrdinalIgnoreCase))
                    .Select(claim => claim.Value)
                    .FirstOrDefault());
            requisicaoContexto.Organizacao = new OrganizacaoId(claimsIdentity?.Claims
                    .Where(claim => claim.Type.Equals(TokenClaims.OrganizacaoId(), StringComparison.OrdinalIgnoreCase))
                    .Select(claim => claim.Value)
                    .FirstOrDefault());
        }
    }
}
