using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace localtour.Messages.Dto
{
    [AutoMapTo(typeof(Message))]
    [AutoMapFrom(typeof(Message))]
    public class MessageDto : EntityDto
    {
        public DateTime DateSent { get; set; }
        public string Content { get; set; }
        public long? ReceiverId { get; set; }
        public long? SenderId { get; set; }
    }
}
