using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Requests.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Requests
{
    public interface IRequestAppService : IApplicationService
    {
        Task<PagedResultDto<GetRequestForViewDto>> GetAll(GetAllRequestsInput input);

        Task<GetRequestForViewDto> GetRequestForView(int id);

        Task<GetRequestForEditOutput> GetRequestForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRequestDto input);

        Task Delete(EntityDto input);
    }
}
