using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Reviews.Dto
{
    public class CreateOrEditReviewDto : EntityDto<int?>
    {
        public long? UserId { get; set; }

        public int? TourId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        public DateTime? DatePosted { get; set; }
    }
}
