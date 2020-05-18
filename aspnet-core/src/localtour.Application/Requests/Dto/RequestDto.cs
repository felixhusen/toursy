using Abp.Application.Services.Dto;
using System;

namespace localtour.Requests.Dto
{
    public class RequestDto : EntityDto
    {
        public int TourId { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? Date { get; set; }
    }
}
