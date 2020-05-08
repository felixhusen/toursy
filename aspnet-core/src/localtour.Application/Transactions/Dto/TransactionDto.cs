using Abp.Application.Services.Dto;
using System;

namespace localtour.Transactions.Dto
{
    public class TransactionDto : EntityDto
    {

        public int? BookingId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public string CVCCode { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
