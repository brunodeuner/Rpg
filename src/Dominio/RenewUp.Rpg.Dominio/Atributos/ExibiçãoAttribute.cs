using System;
using System.Runtime.CompilerServices;
using RenewUp.Rpg.Dominio.Enumerados;

namespace RenewUp.Rpg.Dominio.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExibiçãoAttribute : Attribute
    {
        public ExibiçãoAttribute(string descrição = default, PermissaoCrud permissaoCrud = default,
            [CallerMemberName] string nomeDaPropriedade = default)
        {
            Descrição = descrição ?? nomeDaPropriedade;
            PermissaoCrud = permissaoCrud;
        }

        public string Descrição { get; init; }
        public PermissaoCrud PermissaoCrud { get; init; }
    }
}
