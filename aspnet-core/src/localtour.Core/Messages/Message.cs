using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localtour.Messages
{
    [Table("Messages")]
    public class Message : Entity
    {
        public virtual DateTime DateSent { get; set; }
        public virtual string Content { get; set; }
        public virtual long? ReceiverId { get; set; }
        public virtual long? SenderId { get; set; }
    }
}
