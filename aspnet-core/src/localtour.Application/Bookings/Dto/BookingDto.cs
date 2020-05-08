using Abp.Application.Services.Dto;

namespace localtour.Bookings.Dto
{
    public class BookingDto : EntityDto
    {
        public int? TourId { get; set; }

        public long? UserId { get; set; }

        public string Address { get; set; }

        public int? StateId { get; set; }

        public string Suburb { get; set; }

        public int PostCode { get; set; }

        public string PromoCode { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
