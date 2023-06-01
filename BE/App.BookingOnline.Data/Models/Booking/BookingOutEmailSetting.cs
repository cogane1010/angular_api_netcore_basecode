using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models
{
    public class BookingOutEmailSetting
    {
        public string transaction_out_email_to { get; set; }
        public string transaction_out_email_cc { get; set; }
        public string transaction_out_email_bcc { get; set; }
        public string transaction_out_email_hour { get; set; }
        public string transaction_out_email_dow { get; set; }
        public string HourGetFileIn { get; set; }
    }
}
