using SAPLink.Core.Models;

namespace SAPLink.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Clients> Clients { get; }
        public IBaseRepository<Credentials> Credentials { get; }
        public IBaseRepository<Recurrence> Recurrences { get; }
        public IBaseRepository<Subsidiary> Subsidiary { get; }
        public IBaseRepository<Schedules> Schedule { get; }
        public IBaseRepository<Logger<ItemMasterData>> ItemsLog { get; }
        //public IBaseRepository<Logger<ItemGroups>> ItemGroupsLog { get; }
        //public IBaseRepository<Logger<Vendor>> VendorsLog { get; }

        ///public IBaseRepository<PriceLevel> PriceLevel { get; }
        //public IBaseRepository<Season> Season { get; }
        public IBaseRepository<Sync> Sync { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Clients = new BaseRepository<Clients>(context);
            Credentials = new BaseRepository<Credentials>(context);
            Subsidiary = new BaseRepository<Subsidiary>(context);
            Schedule = new BaseRepository<Schedules>(context);

            Recurrences = new BaseRepository<Recurrence>(context);
            Sync = new BaseRepository<Sync>(context);

            //ItemsLog = new BaseRepository<Logger<ItemMasterData>>(context);
            //ItemGroupsLog = new BaseRepository<Logger<ItemGroups>>(context);
            //VendorsLog = new BaseRepository<Logger<Vendor>>(context);

            //PriceLevel = new BaseRepository<PriceLevel>(context);
            //Season = new BaseRepository<Season>(context);
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}