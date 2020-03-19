using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Tours.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace localtour.Tours
{
    [AbpAuthorize()]
    public class TourAppService : localtourAppServiceBase, ITourAppService
    {
        private readonly IRepository<Tour, int> _tourRepository;

        public TourAppService(IRepository<Tour, int> tourRepository)
        {
            _tourRepository = tourRepository;
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
                            }
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

            return output;
        }

        [AbpAuthorize()]
        public async Task<GetTourForEditOutput> GetStateForEdit(EntityDto input)
        {
            var tour = await _tourRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTourForEditOutput { Tour = ObjectMapper.Map<CreateOrEditTourDto>(tour) };

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

        [AbpAuthorize()]
        protected virtual async Task Create(CreateOrEditTourDto input)
        {
            var tour = ObjectMapper.Map<Tour>(input);


            if (AbpSession.TenantId != null)
            {
                tour.TenantId = (int?)AbpSession.TenantId;
            }


            await _tourRepository.InsertAsync(tour);
        }

        [AbpAuthorize()]
        protected virtual async Task Update(CreateOrEditTourDto input)
        {
            var tour = await _tourRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, tour);
        }

        [AbpAuthorize()]
        public async Task Delete(EntityDto input)
        {
            await _tourRepository.DeleteAsync(input.Id);
        }
    }
}
