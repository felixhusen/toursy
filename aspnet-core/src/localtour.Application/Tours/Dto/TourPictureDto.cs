using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    public class TourPictureDto : EntityDto
    {
        public string Link { get; set; }

        public int? TourId { get; set; }
    }
}
