using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models.Common
{
    public class Hotline : BaseEntity, IEntity
    {
        public string PhoneNumber { get; set; }
     
    }
}
