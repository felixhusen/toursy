using Abp.Domain.Entities;
using localtour.Tours;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.TourPictures
{
    [Table("TourPictures")]
    public class TourPicture : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Link { get; set; }

        public virtual int? TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour TourFk { get; set; }
    }
}
