using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Authorization.Users;
using localtour.Bookings;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Helpers;
using localtour.Requests.Dto;
using localtour.Requests.Exporting;
using localtour.Tours;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Requests
{

    public class RequestAppService : localtourAppServiceBase, IRequestAppService
    {
        private readonly IRepository<Request, int> _requestRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRepository<Tour, int> _tourRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Booking, int> _bookingRepository;
        private readonly IRequestsExcelExporter _requestsExcelExporter;

        public RequestAppService(IRepository<Request, int> requestRepository, IWebHostEnvironment hostEnvironment, IRepository<Tour, int> tourRepository, IRepository<User, long> userRepository, IRepository<Booking, int> bookingRepository, IRequestsExcelExporter requestsExcelExporter)
        {
            _requestRepository = requestRepository;
            _hostEnvironment = hostEnvironment;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _requestsExcelExporter = requestsExcelExporter;
        }

        public async Task<PagedResultDto<GetRequestForViewDto>> GetAll(GetAllRequestsInput input)
        {
            var filteredRequests = _requestRepository.GetAll().AppendRequestMainFilter(input, AbpSession.UserId);

            var requests = from o in filteredRequests

                           join tour in _tourRepository.GetAll() on o.TourId equals tour.Id
                           join user in _userRepository.GetAll() on o.UserId equals user.Id
                           join booking in _bookingRepository.GetAll() on o.BookingId equals booking.Id

                           select new GetRequestForViewDto()
                           {
                               Request = new RequestDto
                               {
                                   Id = o.Id,
                                   TourId = o.TourId,
                                   Description = o.Description,
                                   Status = o.Status,
                                   Date = o.Date,
                                   BookingId = o.BookingId
                               },
                               TourName = tour.Name,
                               UserFullName = user.FullName,
                               BookingCode = "B-" + booking.Id
                           };

            var pagedAndFilteredRequests = requests
                .OrderBy(input.Sorting ?? "Request.Id desc")
                .PageBy(input);

            var totalCount = await requests.CountAsync();

            return new PagedResultDto<GetRequestForViewDto>(
                totalCount,
                await pagedAndFilteredRequests.ToListAsync()
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Edit)]
        public async Task CancelRequest(int id)
        {
            var request = await _requestRepository.GetAsync(id);
            request.Status = "Cancellation Requested";
            await _requestRepository.UpdateAsync(request);
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Edit)]
        public async Task ApproveRequest(int id)
        {
            var request = await _requestRepository.GetAsync(id);
            request.Status = "Success";
            await _requestRepository.UpdateAsync(request);
        }

        public async Task<FileDto> GetRequestsToExcel(GetAllRequestsInput input)
        {
            var filteredRequests = _requestRepository.GetAll().AppendRequestMainFilter(input, AbpSession.UserId);

            var requests = from o in filteredRequests

                           join tour in _tourRepository.GetAll() on o.TourId equals tour.Id
                           join user in _userRepository.GetAll() on o.UserId equals user.Id
                           join booking in _bookingRepository.GetAll() on o.BookingId equals booking.Id

                           where o.UserId == AbpSession.UserId

                           select new GetRequestForViewDto()
                           {
                               Request = new RequestDto
                               {
                                   Id = o.Id,
                                   TourId = o.TourId,
                                   Description = o.Description,
                                   Status = o.Status,
                                   Date = o.Date,
                                   BookingId = o.BookingId
                               },
                               TourName = tour.Name,
                               UserFullName = user.FullName,
                               BookingCode = "B-" + booking.Id
                           };

            var result = await requests
                .OrderBy(input.Sorting ?? "Request.Id asc").ToListAsync();

            return _requestsExcelExporter.ExportToFile(result);
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

        //[AbpAuthorize(PermissionNames.Pages_Request_Create)]
        protected virtual async Task Create(CreateOrEditRequestDto input)
        {
            var request = ObjectMapper.Map<Request>(input);

            request.Status = "Pending";

            request.Date = DateTime.Now;

            request.UserId = AbpSession.UserId;

            if (AbpSession.TenantId != null)
            {
                request.TenantId = (int?)AbpSession.TenantId;
            }


            await _requestRepository.InsertAsync(request);
        }

        //[AbpAuthorize(PermissionNames.Pages_Request_Edit)]
        protected virtual async Task Update(CreateOrEditRequestDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync((int)input.Id);

            ObjectMapper.Map(input, request);

            request.Date = DateTime.Now;
        }

        [AbpAuthorize(PermissionNames.Pages_Request_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _requestRepository.DeleteAsync(input.Id);
        }

    }
}
