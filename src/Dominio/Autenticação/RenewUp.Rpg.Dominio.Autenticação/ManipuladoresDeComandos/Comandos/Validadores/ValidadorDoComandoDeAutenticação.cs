using FluentValidation;

namespace RenewUp.Rpg.Dominio.Autenticação.Comandos.Validadores
{
    public class ValidadorDoComandoDeAutenticação : AbstractValidator<ComandoDeAutenticação>
    {
        public ValidadorDoComandoDeAutenticação() =>
            RuleFor(x => x)
                .Must(CamposInformados)
                .WithMessage("Informe seu email e sua senha");

        private bool CamposInformados(ComandoDeAutenticação comando) =>
            !string.IsNullOrEmpty(comando.Email) && !string.IsNullOrEmpty(comando.Senha);
    }
}
