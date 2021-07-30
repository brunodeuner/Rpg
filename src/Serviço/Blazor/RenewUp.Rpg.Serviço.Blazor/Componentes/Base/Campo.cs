using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Dominio.Atributos;
using RenewUp.Rpg.Dominio.Enumerados;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Base
{
    public sealed class Campo
    {
        private object valor;

        public Campo(object objeto, PropertyInfo propriedade, Action<ChangeEventArgs> onChange)
        {
            var atributoDeExibição = propriedade.PropertyType.GetCustomAttribute<ExibiçãoAttribute>();

            Id = $"{objeto.GetType().FullName}-{propriedade.Name})";
            Objeto = objeto;
            Descrição = atributoDeExibição?.Descrição ?? propriedade.Name;
            Propriedade = propriedade;
            Tipo = propriedade.PropertyType;
            Valor = propriedade.GetValue(objeto);
            OnChange = onChange;
            Desabilitado = atributoDeExibição?.PermissaoCrud.HasFlag(PermissaoCrud.AdicionarAtualizar) ?? false;
        }

        public string Id { get; init; }
        public object Objeto { get; init; }
        public string Descrição { get; init; }
        public PropertyInfo Propriedade { get; init; }
        public Type Tipo { get; init; }
        public Action<ChangeEventArgs> OnChange { get; init; }
        public bool Desabilitado { get; init; }
        public object Valor
        {
            get => valor;
            private set
            {
                valor = value;
                Propriedade.SetValue(Objeto, value);
                OnChange?.Invoke(new ChangeEventArgs()
                {
                    Value = Objeto
                });
            }
        }
        public string ValorEmTexto { get => ObterValor<string>(); set => DefinirValor(value); }
        public bool ValorEmBoolean { get => ObterValor<bool>(); set => DefinirValor(value); }

        private T ObterValor<T>() => (T)Valor;
        private void DefinirValor<T>(T valor) => Valor = valor;
    }
}
