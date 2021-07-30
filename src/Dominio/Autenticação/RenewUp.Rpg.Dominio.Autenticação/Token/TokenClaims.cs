using RenewUp.Rpg.Dominio.Dtos;

namespace RenewUp.Rpg.Dominio.Autenticação.Token
{
    public static class TokenClaims
    {
        public static string UsuarioId() => nameof(Usuario).ToUpperInvariant();
    }
}
