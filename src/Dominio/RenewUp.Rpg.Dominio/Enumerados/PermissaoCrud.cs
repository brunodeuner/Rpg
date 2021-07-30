using System;

namespace RenewUp.Rpg.Dominio.Enumerados
{

    [Flags]
    public enum PermissaoCrud
    {
        None = 0,
        Adicionar = 1,
        Atualizar = 2,
        AdicionarAtualizar = 3,
    }
}
