using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Repositories;


namespace App.BookingOnline.Data.Repositories
{
    public interface ITeeSheetLockRepository : IGridRepository<TeeSheetLock, TeeSheetLockPagingModel>
    {
       
    }
    public interface ITeeSheetLockLineRepository : IGridRepository<TeeSheetLockLine, TeeSheetLockLinePagingModel>
    {

    }
}