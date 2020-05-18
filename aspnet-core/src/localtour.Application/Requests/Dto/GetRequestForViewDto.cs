using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Requests.Dto
{
    public class GetRequestForViewDto
    {
        public RequestDto Request { get; set; }

        public string TourName { get; set; }

        public string UserFullName { get; set; }

        public string BookingCode { get; set; }
    }
}
