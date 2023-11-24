using System;

namespace SAPLink.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepository<Recurrence> Recurrences { get; }
        //IBaseRepository<BPMasterData> BPMasterDataRepository { get; }
        //IItemMasterDataRepository ItemMasterData { get; }
        int SaveChanges();
    }
}