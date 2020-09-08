using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace localtour.Requests.Dto
{
    [AutoMapTo(typeof(Request))]
    [AutoMapFrom(typeof(Request))]
    public class RequestDto : EntityDto
    {
        public int TourId { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? Date { get; set; }

        public int? BookingId { get; set; }
    }
}
