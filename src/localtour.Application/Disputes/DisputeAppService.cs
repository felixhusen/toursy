using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Authorization.Users;
using localtour.Bookings;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Disputes.Dto;
using localtour.Disputes.Exporting;
using localtour.Helpers;
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
        private readonly IDisputesExcelExporter _disputesExcelExporter;

        public DisputeAppService(IRepository<Dispute, int> disputeRepository, IWebHostEnvironment hostEnvironment, IRepository<Tour, int> tourRepository, IRepository<User, long> userRepository, IRepository<Booking, int> bookingRepository, IDisputesExcelExporter disputesExcelExporter)
        {
            _disputeRepository = disputeRepository;
            _hostEnvironment = hostEnvironment;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _disputesExcelExporter = disputesExcelExporter;
        }

        public async Task<PagedResultDto<GetDisputeForViewDto>> GetAll(GetAllDisputesInput input)
        {
            var filteredDisputes = _disputeRepository.GetAll().AppendDisputeMainFilter(input, AbpSession.UserId);

            var disputes = from o in filteredDisputes

                           join booking in _bookingRepository.GetAll() on o.BookingId equals booking.Id
                           join tour in _tourRepository.GetAll() on booking.TourId equals tour.Id
                           join user in _userRepository.GetAll() on booking.UserId equals user.Id

                           select new GetDisputeForViewDto()
                           {
                               Dispute = new DisputeDto
                               {
                                   Id = o.Id,
                                   BookingId = o.BookingId,
                                   Description = o.Description,
                                   Status = o.Status,
                                   Date = o.Date,
                                   UserId = o.UserId
                               },
                               BookingCode = "B-" + booking.Id,
                               TourName = tour.Name,
                               UserFullName = user.FullName
                           };

            var pagedAndFilteredDisputes = disputes
                .OrderBy(input.Sorting ?? "Dispute.Id desc")
                .PageBy(input);

            var totalCount = await disputes.CountAsync();

            return new PagedResultDto<GetDisputeForViewDto>(
                totalCount,
                await pagedAndFilteredDisputes.ToListAsync()
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Dispute_Edit)]
        public async Task CancelDispute(int id)
        {
            var dispute = await _disputeRepository.GetAsync(id);
            dispute.Status = "Cancellation Requested";
            await _disputeRepository.UpdateAsync(dispute);
        }

        [AbpAuthorize(PermissionNames.Pages_Dispute_Edit)]
        public async Task ApproveDispute(int id)
        {
            var dispute = await _disputeRepository.GetAsync(id);
            dispute.Status = "Success";
            await _disputeRepository.UpdateAsync(dispute);
        }

        public async Task<FileDto> GetDisputesToExcel(GetAllDisputesInput input)
        {
            var filteredDisputes = _disputeRepository.GetAll().AppendDisputeMainFilter(input, AbpSession.UserId);

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
                                   Date = o.Date,
                                   UserId = o.UserId
                               },
                               BookingCode = "B-" + booking.Id,
                               TourName = tour.Name,
                               UserFullName = user.FullName
                           };

            var result = await disputes.OrderBy(input.Sorting ?? "Dispute.Id asc").ToListAsync();

            return _disputesExcelExporter.ExportToFile(result);
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

        //[AbpAuthorize(PermissionNames.Pages_Dispute_Create)]
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

        //[AbpAuthorize(PermissionNames.Pages_Dispute_Edit)]
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
