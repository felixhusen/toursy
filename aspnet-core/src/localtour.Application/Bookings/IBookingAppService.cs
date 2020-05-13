using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Bookings.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Bookings
{
    public interface IBookingAppService : IApplicationService
    {
        Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input);

        Task<GetBookingForViewDto> GetBookingForView(int id);

        Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto input);

        Task<BookingDto> CreateOrEdit(CreateOrEditBookingDto input);

        Task Delete(EntityDto input);
    }
}
