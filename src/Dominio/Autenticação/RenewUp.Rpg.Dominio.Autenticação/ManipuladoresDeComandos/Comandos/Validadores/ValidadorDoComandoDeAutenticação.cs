using FluentValidation;

namespace RenewUp.Rpg.Dominio.Autenticação.Comandos.Validadores
{
    public class ValidadorDoComandoDeAutenticação : AbstractValidator<ComandoDeAutenticação>
    {
        public ValidadorDoComandoDeAutenticação() =>
            RuleFor(x => x)
                .Must(EmailESenhaInformados)
                .WithMessage("Informe seu email e sua senha");

        private static bool EmailESenhaInformados(ComandoDeAutenticação comando) =>
            !string.IsNullOrEmpty(comando.Email) && !string.IsNullOrEmpty(comando.Senha);
    }
}
