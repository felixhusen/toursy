using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using localtour.TourDates;
using localtour.TourPictures;
using System;

namespace localtour.Tours.Dto
{
    [AutoMapTo(typeof(TourDate))]
    [AutoMapFrom(typeof(TourDate))]
    public class TourDateDto : EntityDto<int?>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? TourId { get; set; }
    }
}
