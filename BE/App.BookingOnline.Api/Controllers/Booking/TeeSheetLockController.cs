using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;

namespace App.BookingOnline.Api.Controllers
{


    public class TeeSheetLockController : BaseGridController<TeeSheetLockDTO, TeeSheetLockPagingModel, ITeeSheetLockService>
    {

        public TeeSheetLockController(ITeeSheetLockService service) : base(service)
        {

        }

        public override RespondData AddOrEdit(TeeSheetLockDTO entityDTO)
        {
            return base.AddOrEdit(entityDTO);
        }

    }

    public class TeeSheetLockLineController : BaseGridController<TeeSheetLockLineDTO, TeeSheetLockLinePagingModel, ITeeSheetLockLineService>
    {

        public TeeSheetLockLineController(ITeeSheetLockLineService service) : base(service)
        {

        }

    }


}