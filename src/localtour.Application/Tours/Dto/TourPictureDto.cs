using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using localtour.TourPictures;

namespace localtour.Tours.Dto
{
    [AutoMapTo(typeof(TourPicture))]
    [AutoMapFrom(typeof(TourPicture))]
    public class TourPictureDto : EntityDto
    {
        public string Link { get; set; }

        public int? TourId { get; set; }
    }
}
