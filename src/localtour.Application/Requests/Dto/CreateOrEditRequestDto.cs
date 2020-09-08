using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Requests.Dto
{
    [AutoMapTo(typeof(Request))]
    [AutoMapFrom(typeof(Request))]
    public class CreateOrEditRequestDto : EntityDto<int?>
    {
        public int TourId { get; set; }

        public string Description { get; set; }

        public int BookingId { get; set; }
    }
}
