using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Transactions.Dto
{
    public class GetAllTransactionsInput : PagedAndSortedResultRequestDto
    {
        public DateTime? TransactionDate { get; set; }
    }
}
