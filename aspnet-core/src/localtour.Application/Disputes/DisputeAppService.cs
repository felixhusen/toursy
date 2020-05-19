using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Authorization.Users;
using localtour.Bookings;
using localtour.Disputes.Dto;
using localtour.Tours;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Disputes
{

    public class DisputeAppService : localtourAppServiceBase, IDisputeAppService
    {
        private readonly IRepository<Dispute, int> _disputeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRepository<Tour, int> _tourRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Booking, int> _bookingRepository;

        public DisputeAppService(IRepository<Dispute, int> disputeRepository, IWebHostEnvironment hostEnvironment, IRepository<Tour, int> tourRepository, IRepository<User, long> userRepository, IRepository<Booking, int> bookingRepository)
        {
            _disputeRepository = disputeRepository;
            _hostEnvironment = hostEnvironment;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<PagedResultDto<GetDisputeForViewDto>> GetAll(GetAllDisputesInput input)
        {
            var filteredDisputes = _disputeRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Description.Contains(input.Query));

            var disputes = from o in filteredDisputes

                           join booking in _bookingRepository.GetAll() on o.BookingId equals booking.Id
                           join tour in _tourRepository.GetAll() on booking.TourId equals tour.Id
                           join user in _userRepository.GetAll() on booking.UserId equals user.Id

                           where booking.UserId == AbpSession.UserId

                           select new GetDisputeForViewDto()
                           {
                               Dispute = new DisputeDto
                               {
                                   Id = o.Id,
                                   BookingId = o.BookingId,
                                   Description = o.Description,
                                   Status = o.Status,
                                   Date = o.Date
                               },
                               BookingCode = "B-" + booking.Id,
                               TourName = tour.Name,
                               UserFullName = user.FullName
                           };

            var pagedAndFilteredDisputes = disputes
                .OrderBy(input.Sorting ?? "Dispute.Id asc")
                .PageBy(input);

            var totalCount = await disputes.CountAsync();

            return new PagedResultDto<GetDisputeForViewDto>(
                totalCount,
                await pagedAndFilteredDisputes.ToListAsync()
            );
        }

        public async Task<GetDisputeForViewDto> GetDisputeForView(int id)
        {
            var dispute = await _disputeRepository.GetAsync(id);

            var output = new GetDisputeForViewDto { Dispute = ObjectMapper.Map<DisputeDto>(dispute) };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Dispute_Edit)]
        public async Task<GetDisputeForEditOutput> GetDisputeForEdit(EntityDto input)
        {
            var dispute = await _disputeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDisputeForEditOutput { Dispute = ObjectMapper.Map<CreateOrEditDisputeDto>(dispute) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDisputeDto input)
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

        [AbpAuthorize(PermissionNames.Pages_Dispute_Create)]
        protected virtual async Task Create(CreateOrEditDisputeDto input)
        {
            var dispute = ObjectMapper.Map<Dispute>(input);


            if (AbpSession.TenantId != null)
            {
                dispute.TenantId = (int?)AbpSession.TenantId;
            }

            dispute.Status = "Pending";

            dispute.Date = DateTime.Now;

            await _disputeRepository.InsertAsync(dispute);
        }

        [AbpAuthorize(PermissionNames.Pages_Dispute_Edit)]
        protected virtual async Task Update(CreateOrEditDisputeDto input)
        {
            var dispute = await _disputeRepository.FirstOrDefaultAsync((int)input.Id);
            dispute.Date = DateTime.Now;
            ObjectMapper.Map(input, dispute);
        }

        [AbpAuthorize(PermissionNames.Pages_Dispute_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _disputeRepository.DeleteAsync(input.Id);
        }
    }
}
