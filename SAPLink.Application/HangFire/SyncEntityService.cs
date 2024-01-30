using SAPLink.Domain;
using SAPLink.Infrastructure;

namespace SAPLink.Application.HangFire
{
    public static class SyncEntityService
    {
        public static void UpdateSyncEntityDate(UnitOfWork unitOfWork, Enums.UpdateType type)
        {
            var entity = unitOfWork.Sync.Find(x => x.UpdateType == type);
            entity.Date = DateTime.Now;

            unitOfWork.Sync.Update(entity);
            unitOfWork.SaveChanges();
        }

        public static DateTime CompareSyncDateWithDateNow(UnitOfWork unitOfWork, Enums.UpdateType type, out bool isNeedSync)
        {
            var date = unitOfWork.Sync.Find(x => x.UpdateType == type).Date;

            var difference = (int)(DateTime.Now - date).TotalMinutes;

            isNeedSync = difference > 0;

            return date;
        }
    }
}
