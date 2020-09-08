using Abp.Linq.Extensions;
using localtour.Bookings;
using localtour.Bookings.Dto;
using localtour.Tours;
using localtour.Tours.Dto;
using localtour.Transactions.Dto;
using System.Linq;
using localtour.Transactions;
using localtour.Disputes;
using localtour.Disputes.Dto;
using localtour.Requests;
using localtour.Requests.Dto;

namespace localtour.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable<Tour> AppendTourMainFilter(this IQueryable<Tour> existingQuery, GetAllToursInput input)
        {
            return existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Name), tour => tour.Name.Contains(input.Name))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), tour => tour.Name.Contains(input.Description))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.LocationName), tour => tour.LocationName.Contains(input.LocationName))
                                .WhereIf(input.MinPrice != null, tour => tour.Price >= input.MinPrice)
                                .WhereIf(input.MaxPrice != null, tour => tour.Price <= input.MaxPrice)
                                .WhereIf(input.UserId != null, tour => tour.UserId == input.UserId);
        }

        public static IQueryable<Booking> AppendBookingMainFilter(this IQueryable<Booking> existingQuery, GetAllBookingsInput input, long? UserId)
        {
            existingQuery = existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Suburb.Contains(input.Query) || e.Name.Contains(input.Query) || e.Email.Contains(input.Query) || e.Email.Contains(input.Query) || e.Address.Contains(input.Query) || e.TourFk.Name.Contains(input.Query) || e.TourFk.LocationName.Contains(input.Query));

            switch (input.Mode)
            {
                case "CustomerBookings":
                    existingQuery = existingQuery.Where(booking => booking.TourFk.UserId == UserId);
                    break;
                case "CancellationRequests":
                    existingQuery = existingQuery.Where(booking => booking.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(booking => booking.Status == "Cancellation Requested");
                    break;
                case "PendingRequests":
                    existingQuery = existingQuery.Where(booking => booking.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(booking => booking.Status == "Pending");
                    break;
                default:
                    existingQuery = existingQuery.Where(e => e.UserId == UserId);
                    break;
            }

            return existingQuery;
        }

        public static IQueryable<Transaction> AppendTransactionMainFilter(this IQueryable<Transaction> existingQuery, GetAllTransactionsInput input, long? UserId)
        {
            existingQuery = existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.NameOnCard.Contains(input.Query) || e.BookingFk.Name.Contains(input.Query) || e.BookingFk.Email.Contains(input.Query));

            switch (input.Mode)
            {
                case "CustomerTransactions":
                    existingQuery = existingQuery.Where(transaction => transaction.BookingFk.TourFk.UserId == UserId);
                    break;
                case "CancellationRequests":
                    existingQuery = existingQuery.Where(transaction => transaction.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(transaction => transaction.Status == "Cancellation Requested");
                    break;
                case "PendingRequests":
                    existingQuery = existingQuery.Where(transaction => transaction.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(transaction => transaction.Status == "Pending");
                    break;
                default:
                    existingQuery = existingQuery.Where(e => e.BookingFk.UserId == UserId);
                    break;
            }

            return existingQuery;
        }

        public static IQueryable<Dispute> AppendDisputeMainFilter(this IQueryable<Dispute> existingQuery, GetAllDisputesInput input, long? UserId)
        {
            existingQuery = existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Description.Contains(input.Query) || e.Status.Contains(input.Query) || e.BookingFk.Name.Contains(input.Query) || e.BookingFk.TourFk.Name.Contains(input.Query));

            switch (input.Mode)
            {
                case "CustomerDisputes":
                    existingQuery = existingQuery.Where(dispute => dispute.BookingFk.TourFk.UserId == UserId);
                    break;
                case "CancellationRequests":
                    existingQuery = existingQuery.Where(dispute => dispute.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(dispute => dispute.Status == "Cancellation Requested");
                    break;
                case "PendingRequests":
                    existingQuery = existingQuery.Where(dispute => dispute.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(dispute => dispute.Status == "Pending");
                    break;
                default:
                    existingQuery = existingQuery.Where(e => e.BookingFk.UserId == UserId);
                    break;
            }

            return existingQuery;
        }

        public static IQueryable<Request> AppendRequestMainFilter(this IQueryable<Request> existingQuery, GetAllRequestsInput input, long? UserId)
        {
            existingQuery = existingQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Query), e => false || e.Description.Contains(input.Query) || e.BookingFk.Status.Contains(input.Query) || e.BookingFk.Name.Contains(input.Query) || e.BookingFk.TourFk.Name.Contains(input.Query));

            switch (input.Mode)
            {
                case "CustomerRequests":
                    existingQuery = existingQuery.Where(request => request.BookingFk.TourFk.UserId == UserId);
                    break;
                case "CancellationRequests":
                    existingQuery = existingQuery.Where(request => request.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(request => request.Status == "Cancellation Requested");
                    break;
                case "PendingRequests":
                    existingQuery = existingQuery.Where(request => request.BookingFk.TourFk.UserId == UserId);
                    existingQuery = existingQuery.Where(request => request.Status == "Pending");
                    break;
                default:
                    existingQuery = existingQuery.Where(e => e.BookingFk.UserId == UserId);
                    break;
            }

            return existingQuery;
        }
    }
}
