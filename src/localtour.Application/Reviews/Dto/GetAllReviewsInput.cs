using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Reviews.Dto
{
    public class GetAllReviewsInput : PagedAndSortedResultRequestDto
    {
        public string Query { get; set; }
    }
}
