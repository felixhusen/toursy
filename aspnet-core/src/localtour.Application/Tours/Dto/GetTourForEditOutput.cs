using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    public class GetTourForEditOutput
    {
        public CreateOrEditTourDto Tour { get; set; }

        public List<TourPictureDto> TourPictures { get; set; }

        public List<TourDateDto> TourDates { get; set; }
    }
}
