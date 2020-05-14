using Abp.Domain.Entities;
using localtour.Authorization.Users;
using localtour.Bookings;
using localtour.Tours;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Requests
{
    [Table("Requests")]
    public class Request : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        public virtual int TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour TourFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking BookingFk { get; set; }

        public virtual string Description { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? Date { get; set; }
    }
}
