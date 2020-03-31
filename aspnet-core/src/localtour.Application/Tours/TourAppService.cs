using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.TourPictures;
using localtour.Tours.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Tours
{

    public class TourAppService : localtourAppServiceBase, ITourAppService
    {
        private readonly IRepository<Tour, int> _tourRepository;
        private readonly IRepository<TourPicture, int> _tourPictureRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TourAppService(IRepository<Tour, int> tourRepository, IRepository<TourPicture, int> tourPictureRepository, IWebHostEnvironment hostEnvironment)
        {
            _tourRepository = tourRepository;
            _tourPictureRepository = tourPictureRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetTourForViewDto>> GetAll(GetAllToursInput input)
        {
            var filteredTours = _tourRepository.GetAll()
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => false || e.Name.Contains(input.Name))
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => false || e.Name.Contains(input.Description))
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Longitude), e => e.Longitude == input.Longitude)
                                        .WhereIf(!string.IsNullOrWhiteSpace(input.Latitude), e => e.Latitude == input.Latitude);

            var tours = from o in filteredTours

                        select new GetTourForViewDto()
                        {
                            Tour = new TourDto
                            {
                                Id = o.Id,
                                Name = o.Name,
                                Price = o.Price,
                                Description = o.Description,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Latitude = o.Latitude,
                                Longitude = o.Longitude
                            },
                            TourPictures = (

                                from o1 in _tourPictureRepository.GetAll().Where(e => e.TourId == o.Id)
                                select new TourPictureDto
                                {
                                    Id = o1.Id,
                                    Link = o1.Link,
                                    TourId = o1.TourId
                                }

                            ).ToList()
                        };

            var pagedAndFilteredTours = tours
                .OrderBy(input.Sorting ?? "Tour.Id asc")
                .PageBy(input);

            var totalCount = await tours.CountAsync();

            return new PagedResultDto<GetTourForViewDto>(
                totalCount,
                await pagedAndFilteredTours.ToListAsync()
            );
        }

        public async Task<GetTourForViewDto> GetTourForView(int id)
        {
            var tour = await _tourRepository.GetAsync(id);

            var output = new GetTourForViewDto { Tour = ObjectMapper.Map<TourDto>(tour) };

            var tourPictures = from o1 in _tourPictureRepository.GetAll().Where(e => e.TourId == id)

                               select new TourPictureDto
                               {
                                   Id = o1.Id,
                                   Link = o1.Link,
                                   TourId = o1.TourId
                               };

            output.TourPictures = await tourPictures.ToListAsync();

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        public async Task<GetTourForEditOutput> GetTourForEdit(EntityDto input)
        {
            var tour = await _tourRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTourForEditOutput { Tour = ObjectMapper.Map<CreateOrEditTourDto>(tour) };

            var tourPictures = from o1 in _tourPictureRepository.GetAll().Where(e => e.TourId == input.Id)

                               select new TourPictureDto
                               {
                                   Id = o1.Id,
                                   Link = o1.Link,
                                   TourId = o1.TourId
                               };

            output.TourPictures = await tourPictures.ToListAsync();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTourDto input)
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

        [AbpAuthorize(PermissionNames.Pages_Tour_Create)]
        protected virtual async Task Create(CreateOrEditTourDto input)
        {
            var tour = ObjectMapper.Map<Tour>(input);


            if (AbpSession.TenantId != null)
            {
                tour.TenantId = (int?)AbpSession.TenantId;
            }


            await _tourRepository.InsertAsync(tour);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        protected virtual async Task Update(CreateOrEditTourDto input)
        {
            var tour = await _tourRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tour);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tourRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Create)]
        public async Task UploadTourPicture(int TourId, IFormFile file)
        {
            var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, AppConsts.TourPictureUploadPath);

            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            if (file.Length > 0)
            {
                var fileName = $"tourPicture_{TourId}_{DateTime.Now.ToString("hhmmss")}_{file.FileName}";

                var filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var tourPicture = new TourPicture
                {
                    Link = $"http://localhost:21021/{AppConsts.TourPictureUploadPath}/{fileName}",
                    TourId = TourId
                };

                if (AbpSession.TenantId != null)
                {
                    tourPicture.TenantId = (int?)AbpSession.TenantId;
                }

                await _tourPictureRepository.InsertAsync(tourPicture);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        public async Task DeleteTourPicture(int TourPictureId)
        {
            await _tourPictureRepository.DeleteAsync(TourPictureId);
        }

    }
}
