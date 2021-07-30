using MediatR;
using RenewUp.Rpg.Dominio.Atributos;
using SkyInfo.Infra.Bus.Abstracoes;

namespace RenewUp.Rpg.Dominio.Autenticação.Comandos
{
    public class ComandoDeAutenticação : Comando<Unit>
    {
        [Exibição(descrição: "Usuário")]
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
