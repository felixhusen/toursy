using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Requests.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Requests
{

    public class RequestAppService : localtourAppServiceBase, IRequestAppService
    {
        private readonly IRepository<Request, int> _requestRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RequestAppService(IRepository<Request, int> requestRepository, IWebHostEnvironment hostEnvironment)
        {
            _requestRepository = requestRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetRequestForViewDto>> GetAll(GetAllRequestsInput input)
        {
            var filteredRequests = _requestRepository.GetAll()
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Description.Contains(input.Query));

            var requests = from o in filteredRequests

                           select new GetRequestForViewDto()
                           {
                               Request = new RequestDto
                               {
                                   Id = o.Id,
                                   TourId = o.TourId,
                                   Description = o.Description
                               }
                           };

            var pagedAndFilteredRequests = requests
                .OrderBy(input.Sorting ?? "Request.Id asc")
                .PageBy(input);

            var totalCount = await requests.CountAsync();

            return new PagedResultDto<GetRequestForViewDto>(
                totalCount,
                await pagedAndFilteredRequests.ToListAsync()
            );
        }

        public async Task<GetRequestForViewDto> GetRequestForView(int id)
        {
            var request = await _requestRepository.GetAsync(id);

            var output = new GetRequestForViewDto { Request = ObjectMapper.Map<RequestDto>(request) };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Edit)]
        public async Task<GetRequestForEditOutput> GetRequestForEdit(EntityDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRequestForEditOutput { Request = ObjectMapper.Map<CreateOrEditRequestDto>(request) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRequestDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Create)]
        protected virtual async Task Create(CreateOrEditRequestDto input)
        {
            var request = ObjectMapper.Map<Request>(input);


            if (AbpSession.TenantId != null)
            {
                request.TenantId = (int?)AbpSession.TenantId;
            }


            await _requestRepository.InsertAsync(request);
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Edit)]
        protected virtual async Task Update(CreateOrEditRequestDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, request);
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _requestRepository.DeleteAsync(input.Id);
        }

    }
}
