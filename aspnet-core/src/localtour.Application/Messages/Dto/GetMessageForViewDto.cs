using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Messages.Dto
{
    public class GetMessageForViewDto
    {
        public MessageDto Message { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
    }
}
