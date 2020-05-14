using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Reviews.Dto
{
    public class GetReviewForViewDto
    {
        public ReviewDto Review { get; set; }

        public string TourName { get; set; }

        public string UserFullName { get; set; }
    }
}
