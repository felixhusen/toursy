using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Authorization.Users;
using localtour.Bookings.Dto;
using localtour.States;
using localtour.Tours;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Bookings
{

    public class BookingAppService : localtourAppServiceBase, IBookingAppService
    {
        private readonly IRepository<Booking, int> _bookingRepository;
        private readonly IRepository<Tour, int> _tourRepository;
        private readonly IRepository<State, int> _stateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BookingAppService(IRepository<Booking, int> bookingRepository, IRepository<Tour, int> tourRepository, IRepository<State, int> stateRepository, IRepository<User, long> userRepository, IWebHostEnvironment hostEnvironment)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
            _stateRepository = stateRepository;
            _userRepository = userRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input)
        {
            var filteredBookings = _bookingRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Suburb.Contains(input.Query));

            var bookings = from o in filteredBookings

                           join o1 in _tourRepository.GetAll() on o.TourId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _stateRepository.GetAll() on o.StateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                               //join o3 in _userRepository.GetAll() on o.UserId equals o3.Id into j3
                               //from s3 in j3.DefaultIfEmpty()

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
                               },
                               TourName = s1 != null ? s1.Name : null,
                               StateCode = s2 != null ? s2.Code : null,
                               //UserFullName = s3 != null ? s3.FullName : null
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

            var state = await _stateRepository.GetAsync((int)booking.StateId);

            var user = await _userRepository.GetAsync((long)booking.UserId);

            var tour = await _tourRepository.GetAsync((int)booking.TourId);

            var output = new GetBookingForViewDto
            {
                Booking = ObjectMapper.Map<BookingDto>(booking),
                StateCode = state.Code,
                UserFullName = user.FullName,
                TourName = tour.Name
            };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync(input.Id);

            var state = await _stateRepository.GetAsync((int)booking.StateId);

            var user = await _userRepository.GetAsync((long)booking.UserId);

            var tour = await _tourRepository.GetAsync((int)booking.TourId);

            var output = new GetBookingForEditOutput
            {
                Booking = ObjectMapper.Map<CreateOrEditBookingDto>(booking),
                StateCode = state.Code,
                UserFullName = user.FullName,
                TourName = tour.Name
            };

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

        //[AbpAuthorize(PermissionNames.Pages_Booking_Create)]
        protected virtual async Task Create(CreateOrEditBookingDto input)
        {
            try
            {
                var booking = ObjectMapper.Map<Booking>(input);

                if (AbpSession.TenantId != null)
                {
                    booking.TenantId = (int?)AbpSession.TenantId;
                }

                await _bookingRepository.InsertAsync(booking);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
