using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    public class GetTourForViewDto
    {
        public TourDto Tour { get; set; }

        public IEnumerable<TourPictureDto> TourPictures { get; set; }

        public IEnumerable<TourDateDto> TourDates { get; set; }
    }
}
