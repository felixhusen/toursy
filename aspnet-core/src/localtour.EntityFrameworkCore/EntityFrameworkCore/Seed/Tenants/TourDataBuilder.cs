using Abp;
using localtour.Tours;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class TourDataBuilder
    {
        private readonly localtourDbContext _context;

        public TourDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var tours = SeedHelper.SeedData<Tour>("tour_data.json");
            _context.Tours.AddRange(tours);
            _context.SaveChanges();
        }
    }
}
