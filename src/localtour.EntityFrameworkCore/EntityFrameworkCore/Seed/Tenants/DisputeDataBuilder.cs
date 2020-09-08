using Abp.Domain.Uow;
using localtour.Disputes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class DisputeDataBuilder
    {
        private readonly localtourDbContext _context;

        public DisputeDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Disputes.Count() == 0)
            {
                var disputes = SeedHelper.SeedData<Dispute>("disputes.json");
                _context.Disputes.AddRange(disputes);
                _context.SaveChanges();
            }
        }
    }
}
