using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    [AutoMapFrom(typeof(CreateOrEditTourDto), typeof(Tour))]
    public class TourDto : EntityDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string LocationName { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public long? UserId { get; set; }

        public decimal? Rating { get; set; }
    }
}
