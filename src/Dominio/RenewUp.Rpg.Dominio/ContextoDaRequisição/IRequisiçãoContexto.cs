using RenewUp.Rpg.Dominio.Dtos;

namespace RenewUp.Rpg.Dominio.ContextoDaRequisição
{
    public interface IContextoDaRequisição
    {
        UsuarioId Usuario { get; }
        void DefinirContexto(UsuarioId usuario);
        void LimparContexto();
    }
}
