using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Bookings.Dto
{
    public class GetAllBookingsInput : PagedAndSortedResultRequestDto
    {
        public string Query { get; set; }

        public string Mode { get; set; }
    }
}
