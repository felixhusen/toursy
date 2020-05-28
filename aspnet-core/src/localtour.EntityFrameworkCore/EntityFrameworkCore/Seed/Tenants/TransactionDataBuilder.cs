using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Uow;
using localtour.Transactions;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class TransactionDataBuilder
    {
        private readonly localtourDbContext _context;

        public TransactionDataBuilder(localtourDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            if (_context.Transactions.Count() == 0)
            {
                var transactions = SeedHelper.SeedData<Transaction>("transactions.json");
                _context.Transactions.AddRange(transactions);
                _context.SaveChanges();
            }
        }
    }
}
