using Abp.Domain.Entities;
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

        public virtual string Description { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? Date { get; set; }
    }
}
