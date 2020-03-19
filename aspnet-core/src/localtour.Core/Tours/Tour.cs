using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace localtour.Tours
{
    [Table("Tours")]
    public class Tour : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual string Longitude { get; set; }

        public virtual string Latitude { get; set; }
    }
}
