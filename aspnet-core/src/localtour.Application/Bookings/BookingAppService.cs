using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Bookings.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Bookings
{

    public class BookingAppService : localtourAppServiceBase, IBookingAppService
    {
        private readonly IRepository<Booking, int> _bookingRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BookingAppService(IRepository<Booking, int> tourRepository, IWebHostEnvironment hostEnvironment)
        {
            _bookingRepository = tourRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input)
        {
            var filteredBookings = _bookingRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Suburb.Contains(input.Query));

            var bookings = from o in filteredBookings

                           select new GetBookingForViewDto()
                           {
                               Booking = new BookingDto
                               {
                                   Id = o.Id,
                                   TourId = o.TourId,
                                   UserId = o.UserId,
                                   Address = o.Address,
                                   StateId = o.StateId,
                                   Suburb = o.Suburb,
                                   PostCode = o.PostCode,
                                   PromoCode = o.PromoCode,
                                   TotalPrice = o.TotalPrice
                               }
                           };

            var pagedAndFilteredBookings = bookings
                .OrderBy(input.Sorting ?? "Booking.Id asc")
                .PageBy(input);

            var totalCount = await bookings.CountAsync();

            return new PagedResultDto<GetBookingForViewDto>(
                totalCount,
                await pagedAndFilteredBookings.ToListAsync()
            );
        }

        public async Task<GetBookingForViewDto> GetBookingForView(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);

            var output = new GetBookingForViewDto { Booking = ObjectMapper.Map<BookingDto>(booking) };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBookingForEditOutput { Booking = ObjectMapper.Map<CreateOrEditBookingDto>(booking) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBookingDto input)
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

        [AbpAuthorize(PermissionNames.Pages_Booking_Create)]
        protected virtual async Task Create(CreateOrEditBookingDto input)
        {
            var booking = ObjectMapper.Map<Booking>(input);

            if (AbpSession.TenantId != null)
            {
                booking.TenantId = (int?)AbpSession.TenantId;
            }

            await _bookingRepository.InsertAsync(booking);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        protected virtual async Task Update(CreateOrEditBookingDto input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, booking);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _bookingRepository.DeleteAsync(input.Id);
        }

    }
}
