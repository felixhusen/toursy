using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Transactions.Dto
{
    public class GetTransactionForViewDto
    {
        public TransactionDto Transaction { get; set; }

        public string TourName { get; set; }

        public string BookingCode { get; set; }
    }
}
