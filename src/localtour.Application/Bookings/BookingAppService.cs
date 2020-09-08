using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Authorization.Users;
using localtour.Bookings.Dto;
using localtour.Bookings.Exporting;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Helpers;
using localtour.States;
using localtour.TourDates;
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
        private readonly IRepository<TourDate, int> _tourDateRepository;
        private readonly IRepository<State, int> _stateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBookingsExcelExporter _bookingsExcelExporter;

        public BookingAppService(IRepository<Booking, int> bookingRepository, IRepository<Tour, int> tourRepository, IRepository<State, int> stateRepository, IRepository<User, long> userRepository, IWebHostEnvironment hostEnvironment, IBookingsExcelExporter bookingsExcelExporter, IRepository<TourDate, int> tourDateRepository)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
            _stateRepository = stateRepository;
            _userRepository = userRepository;
            _hostEnvironment = hostEnvironment;
            _tourDateRepository = tourDateRepository;
            _bookingsExcelExporter = bookingsExcelExporter;
        }

        public async Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input)
        {
            var filteredBookings = _bookingRepository.GetAll().AppendBookingMainFilter(input, AbpSession.UserId);

            var bookings = from o in filteredBookings

                           join o1 in _tourRepository.GetAll() on o.TourId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join tourDate in _tourDateRepository.GetAll() on o.TourDateId equals tourDate.Id into tourDates
                           from td in tourDates.DefaultIfEmpty()

                           join o2 in _stateRepository.GetAll() on o.StateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _userRepository.GetAll() on o.UserId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

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
                                   TotalPrice = o.TotalPrice,
                                   Name = o.Name,
                                   PhoneNumber = o.PhoneNumber,
                                   NumberOfPeople = o.NumberOfPeople,
                                   Status = o.Status,
                                   Email = o.Email,
                                   TourDateId = o.TourDateId
                               },
                               TourStartDate = td.StartDate,
                               TourEndDate = td.EndDate,
                               BookingCode = "B-" + o.Id,
                               TourName = s1 != null ? s1.Name : null,
                               StateCode = s2 != null ? s2.Code : null,
                               UserFullName = s3 != null ? s3.FullName : null
                           };

            var pagedAndFilteredBookings = bookings
                .OrderBy(input.Sorting ?? "Booking.Id desc")
                .PageBy(input);

            var totalCount = await bookings.CountAsync();

            return new PagedResultDto<GetBookingForViewDto>(
                totalCount,
                await pagedAndFilteredBookings.ToListAsync()
            );
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task CancelBooking(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            booking.Status = "Cancellation Requested";
            await _bookingRepository.UpdateAsync(booking);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task ApproveBooking(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            booking.Status = "Success";
            await _bookingRepository.UpdateAsync(booking);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task ApproveBookingCancellation(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            booking.Status = "Cancelled";
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<FileDto> GetBookingsToExcel(GetAllBookingsInput input)
        {
            var filteredBookings = _bookingRepository.GetAll().AppendBookingMainFilter(input, AbpSession.UserId);

            var bookings = from o in filteredBookings

                           join o1 in _tourRepository.GetAll() on o.TourId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join tourDate in _tourDateRepository.GetAll() on o.TourDateId equals tourDate.Id into tourDates
                           from td in tourDates.DefaultIfEmpty()

                           join o2 in _stateRepository.GetAll() on o.StateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _userRepository.GetAll() on o.UserId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

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
                                   TotalPrice = o.TotalPrice,
                                   Name = o.Name,
                                   PhoneNumber = o.PhoneNumber,
                                   NumberOfPeople = o.NumberOfPeople,
                                   Status = o.Status,
                                   Email = o.Email
                               },
                               TourStartDate = td.StartDate,
                               TourEndDate = td.EndDate,
                               BookingCode = "B-" + o.Id,
                               TourName = s1 != null ? s1.Name : null,
                               StateCode = s2 != null ? s2.Code : null,
                               UserFullName = s3 != null ? s3.FullName : null
                           };

            var results = await bookings.OrderBy(input.Sorting ?? "Booking.Id asc").ToListAsync();

            return _bookingsExcelExporter.ExportToFile(results);
        }

        public async Task RequestCancelBooking(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);

            booking.Status = "Cancellation Requested";
        }

        public async Task<GetBookingForViewDto> GetBookingForView(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);

            var state = await _stateRepository.GetAsync((int)booking.StateId);

            var user = await _userRepository.GetAsync((long)booking.UserId);

            var tour = await _tourRepository.GetAsync((int)booking.TourId);

            var tourDate = await _tourDateRepository.GetAsync((int)booking.TourDateId);

            var output = new GetBookingForViewDto
            {
                Booking = ObjectMapper.Map<BookingDto>(booking),
                StateCode = state.Code,
                UserFullName = user.FullName,
                TourName = tour.Name,
                TourStartDate = tourDate.StartDate,
                TourEndDate = tourDate.EndDate
            };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        public async Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto input)
        {
            try
            {
                var booking = await _bookingRepository.FirstOrDefaultAsync(input.Id);

                var state = await _stateRepository.GetAsync((int)booking?.StateId);

                var user = await _userRepository.GetAsync((long)booking?.UserId);

                var tour = await _tourRepository.GetAsync((int)booking?.TourId);

                var output = new GetBookingForEditOutput
                {
                    Booking = ObjectMapper.Map<CreateOrEditBookingDto>(booking),
                    StateCode = state?.Code,
                    UserFullName = user?.FullName,
                    TourName = tour?.Name
                };

                return output;
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
            
        }

        public async Task<BookingDto> CreateOrEdit(CreateOrEditBookingDto input)
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

        //[AbpAuthorize(PermissionNames.Pages_Booking_Create)]
        protected virtual async Task<BookingDto> Create(CreateOrEditBookingDto input)
        {
            input.Status = "Pending";

            var booking = ObjectMapper.Map<Booking>(input);

            if (AbpSession.TenantId != null)
            {
                booking.TenantId = (int?)AbpSession.TenantId;
            }

            var id = await _bookingRepository.InsertAndGetIdAsync(booking);

            input.Id = id;

            return ObjectMapper.Map<BookingDto>(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Edit)]
        protected virtual async Task<BookingDto> Update(CreateOrEditBookingDto input)
        {
            var booking = await _bookingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, booking);

            return ObjectMapper.Map<BookingDto>(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _bookingRepository.DeleteAsync(input.Id);
        }

    }
}
