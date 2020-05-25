using Abp.Linq.Extensions;
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
    }
}
