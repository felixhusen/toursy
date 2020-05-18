using Abp.Application.Services.Dto;
using System;

namespace localtour.Disputes.Dto
{
    public class DisputeDto : EntityDto
    {

        public int? BookingId { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? Date { get; set; }
    }
}
