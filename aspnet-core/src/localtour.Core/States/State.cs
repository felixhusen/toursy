using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.States
{
    [Table("States")]
    public class State : Entity
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
