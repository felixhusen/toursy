using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Messages.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Messages
{
    public interface IMessageAppService : IApplicationService
    {
        Task<PagedResultDto<GetMessageForViewDto>> GetMessages();

        Task<List<GetMessageForViewDto>> GetMessagesBySender(int SenderId);

        Task<GetMessageForViewDto> SendMessage(CreateMessageDto input);
    }
}
