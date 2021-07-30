using System.Security.Claims;
using SkyInfo.Infra.Bus.Abstracoes;

namespace RenewUp.Rpg.Dominio.Autenticação.Eventos
{
    public class EventoDeUsuarioAutenticadoComSucesso : Evento
    {
        public EventoDeUsuarioAutenticadoComSucesso(ClaimsPrincipal claimsPrincipal) =>
            ClaimsPrincipal = claimsPrincipal;

        public ClaimsPrincipal ClaimsPrincipal { get; private set; }
    }
}
