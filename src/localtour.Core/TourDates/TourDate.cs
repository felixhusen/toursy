using Abp.Domain.Entities;
using localtour.Tours;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.TourDates
{
    [Table("TourDates")]
    public class TourDate : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual int? TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour TourFk { get; set; }
    }
}
