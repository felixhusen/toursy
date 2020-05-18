using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Tours.Dto;
using Microsoft.AspNetCore.Http;
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

        Task<GetTourForEditOutput> GetTourForEdit(EntityDto input);

        Task<TourDto> CreateOrEdit(CreateOrEditTourDto input);

        Task Delete(EntityDto input);

        Task<TourPictureDto> UploadTourPicture(int? TourId, IFormFile file);

        Task DeleteTourPicture(int TourPictureId);
    }
}
