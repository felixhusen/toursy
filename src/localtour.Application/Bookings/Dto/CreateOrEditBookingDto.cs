using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace localtour.Bookings.Dto
{
    [AutoMapTo(typeof(Booking), typeof(BookingDto))]
    [AutoMapFrom(typeof(Booking))]
    public class CreateOrEditBookingDto : EntityDto<int?>
    {
        public int? TourId { get; set; }

        public int? TourDateId { get; set; }

        public long? UserId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int NumberOfPeople { get; set; }

        public string Status { get; set; }

        public string Address { get; set; }

        public int? StateId { get; set; }

        public string Suburb { get; set; }

        public int PostCode { get; set; }

        public string PromoCode { get; set; }

        public decimal TotalPrice { get; set; }

        public string Email { get; set; }
    }
}
