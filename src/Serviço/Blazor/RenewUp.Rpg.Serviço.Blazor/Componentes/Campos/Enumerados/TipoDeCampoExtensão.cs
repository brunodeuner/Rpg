using System;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Campos.Enumerados
{
    public static class TipoDeCampoExtensão
    {
        public static TipoDeCampo ObterAPartirDoTipo(this Type tipo)
        {
            if (tipo == typeof(string))
                return TipoDeCampo.Texto;
            if (tipo == typeof(bool))
                return TipoDeCampo.CheckBox;
            throw new ArgumentOutOfRangeException(nameof(tipo));
        }
    }
}
