using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Transactions.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Transactions
{

    public class TransactionAppService : localtourAppServiceBase, ITransactionAppService
    {
        private readonly IRepository<Transaction, int> _transactionRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TransactionAppService(IRepository<Transaction, int> transactionRepository, IWebHostEnvironment hostEnvironment)
        {
            _transactionRepository = transactionRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input)
        {
            var filteredTransactions = _transactionRepository.GetAll()
                                        .WhereIf(input.TransactionDate != null, e => false || e.TransactionDate == input.TransactionDate);

            var transactions = from o in filteredTransactions

                        select new GetTransactionForViewDto()
                        {
                            Transaction = new TransactionDto
                            {
                                Id = o.Id,
                                TransactionDate = o.TransactionDate,
                                Amount = o.Amount,
                                BookingId = o.BookingId,
                                CardNumber = o.CardNumber,
                                CVCCode = o.CVCCode
                            }
                        };

            var pagedAndFilteredTransactions = transactions
                .OrderBy(input.Sorting ?? "Transaction.Id asc")
                .PageBy(input);

            var totalCount = await transactions.CountAsync();

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
                await pagedAndFilteredTransactions.ToListAsync()
            );
        }

        public async Task<GetTransactionForViewDto> GetTransactionForView(int id)
        {
            var transaction = await _transactionRepository.GetAsync(id);

            var output = new GetTransactionForViewDto { Transaction = ObjectMapper.Map<TransactionDto>(transaction) };

            return output;
        }

        [AbpAuthorize(PermissionNames.Pages_Transaction_Edit)]
        public async Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto input)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionForEditOutput { Transaction = ObjectMapper.Map<CreateOrEditTransactionDto>(transaction) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTransactionDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Transaction_Create)]
        protected virtual async Task Create(CreateOrEditTransactionDto input)
        {
            var transaction = ObjectMapper.Map<Transaction>(input);


            if (AbpSession.TenantId != null)
            {
                transaction.TenantId = (int?)AbpSession.TenantId;
            }


            await _transactionRepository.InsertAsync(transaction);
        }

        [AbpAuthorize(PermissionNames.Pages_Transaction_Edit)]
        protected virtual async Task Update(CreateOrEditTransactionDto input)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transaction);
        }

        [AbpAuthorize(PermissionNames.Pages_Transaction_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _transactionRepository.DeleteAsync(input.Id);
        }

    }
}
