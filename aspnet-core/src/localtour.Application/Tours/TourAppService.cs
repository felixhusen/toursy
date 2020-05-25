using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Helpers;
using localtour.TourDates;
using localtour.TourPictures;
using localtour.Tours.Dto;
using localtour.Tours.Exporting;
using localtour.Users;
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
        private readonly IRepository<TourDate, int> _tourDateRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserAppService _userAppService;
        private readonly IToursExcelExporter _toursExcelExporter;
        public TourAppService(IRepository<Tour, int> tourRepository, IRepository<TourPicture, int> tourPictureRepository, IRepository<TourDate, int> tourDateRepository, IWebHostEnvironment hostEnvironment, IUserAppService userAppService, IToursExcelExporter toursExcelExporter)
        {
            _tourRepository = tourRepository;
            _tourPictureRepository = tourPictureRepository;
            _tourDateRepository = tourDateRepository;
            _userAppService = userAppService;
            _hostEnvironment = hostEnvironment;
            _toursExcelExporter = toursExcelExporter;
        }

        public async Task<PagedResultDto<GetTourForViewDto>> GetAll(GetAllToursInput input)
        {
            try
            {
                var filteredTours = _tourRepository.GetAll().AppendTourMainFilter(input);

                var tours = from o in filteredTours

                            select new GetTourForViewDto()
                            {
                                Tour = new TourDto
                                {
                                    Id = o.Id,
                                    Name = o.Name,
                                    Price = o.Price,
                                    Description = o.Description,
                                    Latitude = o.Latitude,
                                    Longitude = o.Longitude,
                                    LocationName = o.LocationName,
                                    UserId = o.UserId
                                },
                                TourPictures = _tourPictureRepository.GetAll().Where(e => e.TourId == o.Id).Select(e => new TourPictureDto
                                {
                                    Id = e.Id,
                                    Link = e.Link,
                                    TourId = e.Id
                                }),
                                TourDates = _tourDateRepository.GetAll().Where(e => e.TourId == o.Id).OrderByDescending(e => e.StartDate).Select(e => new TourDateDto
                                {
                                    Id = e.Id,
                                    StartDate = e.StartDate,
                                    EndDate = e.EndDate,
                                    TourId = e.Id
                                })
                            };

                var pagedAndFilteredTours = tours
                    .OrderBy(input.Sorting ?? "Tour.Id asc")
                    .PageBy(input);

                var totalCount = await tours.CountAsync();

                return new PagedResultDto<GetTourForViewDto>(
                    totalCount,
                    await pagedAndFilteredTours.ToListAsync()
                );
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public async Task<FileDto> GetToursToExcel(GetAllToursInput input)
        {
            var filteredTours = _tourRepository.GetAll().AppendTourMainFilter(input);

            var tours = from o in filteredTours

                        select new GetTourForViewDto()
                        {
                            Tour = new TourDto
                            {
                                Id = o.Id,
                                Name = o.Name,
                                Price = o.Price,
                                Description = o.Description,
                                Latitude = o.Latitude,
                                Longitude = o.Longitude,
                                LocationName = o.LocationName,
                                UserId = o.UserId
                            },
                            TourPictures = _tourPictureRepository.GetAll().Where(e => e.TourId == o.Id).Select(e => new TourPictureDto
                            {
                                Id = e.Id,
                                Link = e.Link,
                                TourId = e.Id
                            }),
                            TourDates = _tourDateRepository.GetAll().Where(e => e.TourId == o.Id).OrderByDescending(e => e.StartDate).Select(e => new TourDateDto
                            {
                                Id = e.Id,
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                TourId = e.Id
                            })
                        };

            var result = await tours.OrderBy(input.Sorting ?? "Tour.Id asc").ToListAsync();

            return _toursExcelExporter.ExportToFile(result);

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

            var tourDates = from o1 in _tourDateRepository.GetAll().Where(e => e.TourId == id)

                            select new TourDateDto
                            {
                                Id = o1.Id,
                                StartDate = o1.StartDate,
                                EndDate = o1.EndDate
                            };

            output.TourDates = await tourDates.ToListAsync();

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

            var tourDates = from o1 in _tourDateRepository.GetAll().Where(e => e.TourId == input.Id)

                            select new TourDateDto
                            {
                                Id = o1.Id,
                                StartDate = o1.StartDate,
                                EndDate = o1.EndDate
                            };

            output.TourDates = await tourDates.ToListAsync();

            output.TourPictures = await tourPictures.ToListAsync();

            return output;
        }

        public async Task<TourDateDto> CreateOrEditTourDate(TourDateDto input)
        {
            if (input.Id == null)
            {
                return await CreateTourDate(input);
            }
            else
            {
                return await UpdateTourDate(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Create)]
        protected virtual async Task<TourDateDto> CreateTourDate(TourDateDto input)
        {
            var tourDate = ObjectMapper.Map<TourDate>(input);

            if (AbpSession.TenantId != null)
            {
                tourDate.TenantId = (int?)AbpSession.TenantId;
            }

            var tourDateId = await _tourDateRepository.InsertAndGetIdAsync(tourDate);

            input.Id = tourDateId;

            return input;
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        protected virtual async Task<TourDateDto> UpdateTourDate(TourDateDto input)
        {
            var tourDate = await _tourDateRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tourDate);
            return input;
        }

        public async Task<TourDto> CreateOrEdit(CreateOrEditTourDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Create)]
        protected virtual async Task<TourDto> Create(CreateOrEditTourDto input)
        {
            try
            {
                var tour = ObjectMapper.Map<Tour>(input);

                if (AbpSession.TenantId != null)
                {
                    tour.TenantId = (int?)AbpSession.TenantId;
                }

                var tourId = await _tourRepository.InsertAndGetIdAsync(tour);

                var result = ObjectMapper.Map<TourDto>(input);

                result.Id = tourId;

                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        protected virtual async Task<TourDto> Update(CreateOrEditTourDto input)
        {
            var tour = await _tourRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tour);
            return ObjectMapper.Map<TourDto>(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tourRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Delete)]
        public async Task DeleteTourDate(EntityDto input)
        {
            await _tourDateRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Create)]
        public async Task<TourPictureDto> UploadTourPicture(int? TourId, IFormFile file)
        {
            try
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

                    var tourPictureId = await _tourPictureRepository.InsertAndGetIdAsync(tourPicture);

                    var result = ObjectMapper.Map<TourPictureDto>(tourPicture);

                    result.Id = tourPictureId;

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        public async Task UpdateTourPicture(TourPictureDto input)
        {
            var tourPicture = await _tourPictureRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, tourPicture);
        }

        [AbpAuthorize(PermissionNames.Pages_Tour_Edit)]
        public async Task DeleteTourPicture(int TourPictureId)
        {
            await _tourPictureRepository.DeleteAsync(TourPictureId);
        }

    }
}
