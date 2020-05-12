using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Bookings.Dto
{
    public class GetBookingForEditOutput
    {
        public CreateOrEditBookingDto Booking { get; set; }

        public string TourName { get; set; }

        public string UserFullName { get; set; }

        public string StateCode { get; set; }
    }
}
