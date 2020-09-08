using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Disputes.Dto
{
    public class GetDisputeForViewDto
    {
        public DisputeDto Dispute { get; set; }

        public string BookingCode { get; set; }

        public string TourName { get; set; }

        public string UserFullName { get; set; }
    }
}
