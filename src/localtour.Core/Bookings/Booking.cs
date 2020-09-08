using Abp.Domain.Entities;
using localtour.Authorization.Users;
using localtour.TourDates;
using localtour.Tours;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Bookings
{
    [Table("Bookings")]
    public class Booking : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual int? TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour TourFk { get; set; }

        public virtual int? TourDateId { get; set; }

        [ForeignKey("TourDateId")]
        public TourDate TourDateFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual string Address { get; set; }

        public virtual string Name { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual int? StateId { get; set; }

        public virtual string Suburb { get; set; }

        public virtual int PostCode { get; set; }

        public virtual int NumberOfPeople { get; set; }

        public virtual string PromoCode { get; set; }

        public virtual decimal TotalPrice { get; set; }

        public virtual string Status { get; set; }

        public virtual string Email { get; set; }
    }
}
