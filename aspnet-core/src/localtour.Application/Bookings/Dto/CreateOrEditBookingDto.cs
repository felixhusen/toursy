using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Bookings.Dto
{
    public class CreateOrEditBookingDto : EntityDto<int?>
    {
        public int? TourId { get; set; }

        public long? UserId { get; set; }

        public string Address { get; set; }

        public int? StateId { get; set; }

        public string Suburb { get; set; }

        public int PostCode { get; set; }

        public string PromoCode { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
