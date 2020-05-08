using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Requests.Dto
{
    public class GetAllRequestsInput : PagedAndSortedResultRequestDto
    {
        public string Query { get; set; }
    }
}
