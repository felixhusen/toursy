using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace localtour.Reviews.Dto
{
    [AutoMapTo(typeof(Review))]
    [AutoMapFrom(typeof(Review))]
    public class ReviewDto : EntityDto
    {

        public long? UserId { get; set; }

        public int? TourId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

        public DateTime? DatePosted { get; set; }
    }
}
