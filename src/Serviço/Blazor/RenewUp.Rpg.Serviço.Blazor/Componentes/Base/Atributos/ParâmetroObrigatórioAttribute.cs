using System;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Base.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParâmetroObrigatórioAttribute : Attribute { }
}
