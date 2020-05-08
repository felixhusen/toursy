using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Transactions.Dto
{
    public class CreateOrEditTransactionDto : EntityDto<int?>
    {

        public int? BookingId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public string CVCCode { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
