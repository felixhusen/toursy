using Abp.Domain.Uow;
using localtour.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class RequestDataBuilder
    {
        private readonly localtourDbContext _context;

        public RequestDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Requests.Count() == 0)
            {
                var requests = SeedHelper.SeedData<Request>("requests.json");
                _context.Requests.AddRange(requests);
                _context.SaveChanges();
            }
        }
    }
}
