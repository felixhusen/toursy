using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace localtour.Transactions.Dto
{
    [AutoMapFrom(typeof(CreateOrEditTransactionDto), typeof(Transaction))]
    public class TransactionDto : EntityDto
    {
        public int? BookingId { get; set; }

        public decimal Amount { get; set; }

        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string CVCCode { get; set; }

        public DateTime? TransactionDate { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public string Status { get; set; }
    }
}
