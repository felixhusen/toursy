using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using localtour.Bookings;
using localtour.Bookings.Dto;
using localtour.Tours;
using localtour.Tours.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace localtour.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable<Tour> AppendTourMainFilter(this IQueryable<Tour> existingQuery, GetAllToursInput input)
        {
            return existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Name), tour => tour.Name.Contains(input.Name))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), tour => tour.Name.Contains(input.Description))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Longitude), tour => tour.Longitude == input.Longitude)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Latitude), tour => tour.Latitude == input.Latitude)
                                .WhereIf(input.UserId != null, tour => tour.UserId == input.UserId);
        }

        public static IQueryable<Booking> AppendBookingMainFilter(this IQueryable<Booking> existingQuery, GetAllBookingsInput input, long? UserId)
        {
            return existingQuery.Where(e => e.UserId == UserId).WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Suburb.Contains(input.Query) || e.Name.Contains(input.Query) || e.Email.Contains(input.Query) || e.Email.Contains(input.Query) || e.Address.Contains(input.Query) || e.TourFk.Name.Contains(input.Query) || e.TourFk.LocationName.Contains(input.Query));
        }
    }
}
