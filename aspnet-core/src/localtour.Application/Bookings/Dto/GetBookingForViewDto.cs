using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Bookings.Dto
{
    public class GetBookingForViewDto
    {
        public BookingDto Booking { get; set; }

        public string TourName { get; set; }

        public string UserFullName { get; set; }

        public string StateCode { get; set; }
    }
}
