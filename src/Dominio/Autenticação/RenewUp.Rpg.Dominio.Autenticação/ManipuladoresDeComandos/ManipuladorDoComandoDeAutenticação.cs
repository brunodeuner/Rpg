using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using RenewUp.Rpg.Dominio.Autenticação.Comandos;
using RenewUp.Rpg.Dominio.Autenticação.Eventos;
using SkyInfo.Infra.Bus.Abstracoes;

namespace RenewUp.Rpg.Dominio.Autenticação.ComandosManipuladores
{
    public class ManipuladorDoComandoDeAutenticação : IRequestHandler<ComandoDeAutenticação>
    {
        private readonly IMediatorHandler bus;

        public ManipuladorDoComandoDeAutenticação(IMediatorHandler bus) => this.bus = bus;

        public async Task<Unit> Handle(ComandoDeAutenticação request, CancellationToken cancellationToken)
        {
            if (!ValidarTokenEObterDataDeExpiração(request.Email, out var claimsPrincipal))
                return default;

            await bus.PublicarEvento(new EventoDeUsuarioAutenticadoComSucesso(claimsPrincipal), cancellationToken);
            return default;
        }

        private static bool ValidarTokenEObterDataDeExpiração(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = default;
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                SignatureValidator = (string token, TokenValidationParameters parameters) =>
                {
                    var jwt = new JwtSecurityToken(token);

                    return jwt;
                },
                ValidateIssuer = false,
                ValidateLifetime = true,
            };
            if (string.IsNullOrEmpty(token))
                return false;
            try
            {
                claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters,
                    out var _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
