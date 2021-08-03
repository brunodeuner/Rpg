using RenewUp.Rpg.Dominio.Dtos;

namespace RenewUp.Rpg.Dominio.ContextoDaRequisição
{
    public class ContextoDaRequisição : IContextoDaRequisição
    {
        public UsuarioId Usuario { get; private set; }

        public void DefinirContexto(UsuarioId usuario) => Usuario = usuario;

        public void LimparContexto() => DefinirContexto(default);

        public bool Autenticado() => Usuario is not null;

        public bool NãoAutenticado() => !Autenticado();
    }
}
