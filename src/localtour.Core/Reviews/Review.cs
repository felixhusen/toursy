using Abp.Domain.Entities;
using localtour.Authorization.Users;
using localtour.Tours;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Reviews
{
    [Table("Reviews")]
    public class Review : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int? TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour TourFk { get; set; }

        public virtual int Rating { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? DatePosted { get; set; }
    }
}
