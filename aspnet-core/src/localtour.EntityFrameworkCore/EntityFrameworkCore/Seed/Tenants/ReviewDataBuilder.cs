using Abp.Domain.Uow;
using localtour.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class ReviewDataBuilder
    {
        private readonly localtourDbContext _context;

        public ReviewDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Reviews.Count() == 0)
            {
                var reviews = SeedHelper.SeedData<Review>("reviews.json");
                _context.Reviews.AddRange(reviews);
                _context.SaveChanges();
            }
        }
    }
}
