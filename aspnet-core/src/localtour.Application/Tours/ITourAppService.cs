using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Tours.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Tours
{
    public interface ITourAppService : IApplicationService
    {
        Task<PagedResultDto<GetTourForViewDto>> GetAll(GetAllToursInput input);

        Task<GetTourForViewDto> GetTourForView(int id);

        Task<GetTourForEditOutput> GetStateForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTourDto input);

        Task Delete(EntityDto input);
    }
}
