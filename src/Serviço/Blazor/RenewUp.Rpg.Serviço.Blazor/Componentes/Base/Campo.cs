using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Dominio.Atributos;
using RenewUp.Rpg.Dominio.Enumerados;
using System;
using System.Reflection;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Base
{
    public class Campo
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

        public string Id { get; private set; }
        public object Objeto { get; private set; }
        public string Descrição { get; private set; }
        public PropertyInfo Propriedade { get; private set; }
        public Type Tipo { get; private set; }
        public object Valor
        {
            get => valor; private set
            {
                valor = value;
                Propriedade.SetValue(Objeto, value);
                OnChange?.Invoke(new ChangeEventArgs()
                {
                    Value = Objeto
                });
            }
        }
        public Action<ChangeEventArgs> OnChange { get; private set; }
        public bool Desabilitado { get; private set; }
        public string ValorEmTexto { get => ObterValor<string>(); set => DefinirValor(value); }
        public bool ValorEmBoolean { get => ObterValor<bool>(); set => DefinirValor(value); }

        private T ObterValor<T>() => Valor == default ? default : (T)Valor;
        private void DefinirValor<T>(T valor) => Valor = valor;
    }
}
