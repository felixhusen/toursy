using Abp.Domain.Entities;
using localtour.Bookings;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Transactions
{
    [Table("Transactions")]
    public class Transaction : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking BookingFk { get; set; }

        public virtual string NameOnCard { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual string CardNumber { get; set; }

        public virtual string CVCCode { get; set; }

        public virtual int ExpMonth { get; set; }

        public virtual int ExpYear { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

        public virtual string Status { get; set; }
    }
}
