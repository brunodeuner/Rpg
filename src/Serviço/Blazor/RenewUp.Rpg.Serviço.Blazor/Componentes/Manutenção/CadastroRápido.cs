using Microsoft.AspNetCore.Components;
using RenewUp.Rpg.Dominio.Repositorios;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base;
using RenewUp.Rpg.Serviço.Blazor.Componentes.Base.Atributos;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;
using System.Threading;
using System.Threading.Tasks;

namespace RenewUp.Rpg.Serviço.Blazor.Componentes.Manutenção
{
    public partial class CadastroRápido<T> : ComponenteBase
        where T : class, IId, new()
    {
        private bool botõesDesabilitados;

        [Inject]
        public RepositorioBase<T> Repositorio { get; set; }
        public T Entidade { get; set; }
        [Parameter, ParâmetroObrigatório]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool Exibir { get; set; }

        public Task Cadastrar(CancellationToken cancellationToken) =>
            Task.Delay(1000, cancellationToken);
    }
}
