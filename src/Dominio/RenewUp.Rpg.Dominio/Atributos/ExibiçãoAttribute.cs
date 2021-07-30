using System;
using System.Runtime.CompilerServices;
using RenewUp.Rpg.Dominio.Enumerados;

namespace RenewUp.Rpg.Dominio.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExibiçãoAttribute: Attribute
    {
        public ExibiçãoAttribute(string descrição = default, PermissaoCrud permissaoCrud = default, 
            [CallerMemberName] string nomeDaPropriedade = default)
        {
            Descrição = descrição ?? nomeDaPropriedade;
            PermissaoCrud = permissaoCrud;
        }

        public string Descrição { get; private set; }
        public PermissaoCrud PermissaoCrud { get; private set; }
    }
}
