using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Reviews.Dto
{
    [AutoMapTo(typeof(Review))]
    [AutoMapFrom(typeof(Review))]
    public class CreateOrEditReviewDto : EntityDto<int?>
    {
        public long? UserId { get; set; }

        public int? TourId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        public DateTime? DatePosted { get; set; }
    }
}
