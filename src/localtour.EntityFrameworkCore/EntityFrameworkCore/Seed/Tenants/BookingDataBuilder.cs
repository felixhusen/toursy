using Abp.Domain.Uow;
using localtour.Bookings;
using System.Linq;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class BookingDataBuilder
    {
        private readonly localtourDbContext _context;

        public BookingDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Bookings.Count() == 0)
            {
                var bookings = SeedHelper.SeedData<Booking>("bookings.json");
                _context.Bookings.AddRange(bookings);
                _context.SaveChanges();
            }
        }
    }
}
