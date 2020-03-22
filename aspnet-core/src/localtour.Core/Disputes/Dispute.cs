using Abp.Domain.Entities;
using localtour.Bookings;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Disputes
{
    [Table("Disputes")]
    public class Dispute : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking BookingFk { get; set; }

        public virtual string Description { get; set; }
    }
}
