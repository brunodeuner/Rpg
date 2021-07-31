using SkyInfo.Infra.Armazenamento.Abstracoes.Dao;
using SkyInfo.Infra.Armazenamento.Abstracoes.Id;
using SkyInfo.Infra.Armazenamento.Abstracoes.Repositorio;

namespace RenewUp.Rpg.Dominio.Repositorios
{
    public class RepositorioBase<TEntidade> : RepositorioAsync<TEntidade>
        where TEntidade : class, IId, new()
    {
        public RepositorioBase(IDao dao) : base(dao)
        {
        }
    }
}
