using Abp;
using Abp.Domain.Uow;
using localtour.TourDates;
using localtour.TourPictures;
using localtour.Tours;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq.Dynamic.Core;

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
            if (_context.Tours.Count() == 0)
            {
                var tours = SeedHelper.SeedData<Tour>("tour_data.json");
                _context.Tours.AddRange(tours);
                _context.SaveChanges();

                var tourPictures = SeedHelper.SeedData<TourPicture>("tour_pictures.json");
                _context.TourPictures.AddRange(tourPictures);
                _context.SaveChanges();

                var tourDates = SeedHelper.SeedData<TourDate>("tour_dates.json");
                _context.TourDates.AddRange(tourDates);
                _context.SaveChanges();
            }
            
        }
    }
}
