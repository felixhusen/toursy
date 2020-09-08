using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Disputes.Dto
{
    public class GetAllDisputesInput : PagedAndSortedResultRequestDto
    {
        public string Query { get; set; }

        public string Mode { get; set; }
    }
}
