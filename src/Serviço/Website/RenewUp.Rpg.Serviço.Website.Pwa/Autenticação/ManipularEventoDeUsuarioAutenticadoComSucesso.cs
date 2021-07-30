using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RenewUp.Rpg.Dominio.Autenticação.Eventos;

namespace RenewUp.Rpg.Serviço.Website.Pwa.Autenticação
{
    public class ManipularEventoDeUsuarioAutenticadoComSucesso :
        INotificationHandler<EventoDeUsuarioAutenticadoComSucesso>
    {
        private readonly EstadoDaAutenticação estadoDaAutenticação;

        public ManipularEventoDeUsuarioAutenticadoComSucesso(EstadoDaAutenticação estadoDaAutenticação) =>
            this.estadoDaAutenticação = estadoDaAutenticação;

        public Task Handle(EventoDeUsuarioAutenticadoComSucesso notification, CancellationToken cancellationToken) =>
            estadoDaAutenticação.DefinirComoAutenticado(notification.ClaimsPrincipal);
    }
}
