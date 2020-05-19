using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Messages.Dto
{
    public class CreateMessageDto
    {
        public string Content { get; set; }
        public long? ReceiverId { get; set; }
    }
}
