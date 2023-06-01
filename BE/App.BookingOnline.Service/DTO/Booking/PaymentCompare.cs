using App.BookingOnline.Data.Models;

namespace App.BookingOnline.Service.DTO
{
    public class PaymentCompare
    {
        public InTransactionDetailsDTO InTransactionDetails { get; set; }

        public BookingDTO BookingDTO { get; set; }

        public string Rc_code { get; set; }

        public string UserRc_code { get; set; }

        public string old_userRc_code { get; set; } // check ui changed
        public bool IsSummary { get; set; }

    }


}
