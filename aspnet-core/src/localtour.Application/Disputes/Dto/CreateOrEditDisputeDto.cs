using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Disputes.Dto
{
    [AutoMapTo(typeof(Dispute))]
    [AutoMapFrom(typeof(Dispute))]
    public class CreateOrEditDisputeDto : EntityDto<int?>
    {
        public int? BookingId { get; set; }

        public string Description { get; set; }
    }
}
