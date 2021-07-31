namespace RenewUp.Rpg.Infraestrutura
{
    public static class Texto
    {
        public static bool NãoPreenchido(this string texto) => Preenchido(texto);
        public static bool Preenchido(this string texto) => !string.IsNullOrWhiteSpace(texto);
    }
}
