﻿using System;
using SAPLink.Domain.System;

namespace SAPLink.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Recurrence> Recurrences { get; }
        //IBaseRepository<BPMasterData> BPMasterDataRepository { get; }
        //IItemMasterDataRepository ItemMasterData { get; }
        int SaveChanges();
    }
}