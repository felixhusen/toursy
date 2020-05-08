using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Disputes.Dto
{
    public class CreateOrEditDisputeDto : EntityDto<int?>
    {
        public int? BookingId { get; set; }

        public string Description { get; set; }
    }
}
