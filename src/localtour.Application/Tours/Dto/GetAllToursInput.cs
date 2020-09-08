using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    public class GetAllToursInput : PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string LocationName { get; set; }

        public long? UserId { get; set; }
    }
}
