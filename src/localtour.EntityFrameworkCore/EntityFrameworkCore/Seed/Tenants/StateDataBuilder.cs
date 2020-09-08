using Abp.Domain.Uow;
using localtour.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class StateDataBuilder
    {
        private readonly localtourDbContext _context;

        public StateDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.States.Count() == 0)
            {
                var states = SeedHelper.SeedData<State>("states.json");
                _context.States.AddRange(states);
                _context.SaveChanges();
            }
        }
    }
}
