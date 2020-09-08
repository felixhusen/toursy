using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Reviews.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Reviews
{
    public interface IReviewAppService : IApplicationService
    {
        Task<PagedResultDto<GetReviewForViewDto>> GetAll(GetAllReviewsInput input);

        Task<GetReviewForViewDto> GetReviewForView(int id);

        Task<GetReviewForEditOutput> GetReviewForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditReviewDto input);

        Task Delete(EntityDto input);
    }
}
