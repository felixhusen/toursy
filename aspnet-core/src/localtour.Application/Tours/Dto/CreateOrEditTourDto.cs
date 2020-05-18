using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace localtour.Tours.Dto
{
    [AutoMapTo(typeof(Tour))]
    [AutoMapFrom(typeof(Tour))]
    public class CreateOrEditTourDto : EntityDto<int?>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public long? UserId { get; set; }

        public string LocationName { get; set; }
    }
}
