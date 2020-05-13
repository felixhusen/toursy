using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using localtour.Authorization;
using localtour.Bookings;
using localtour.Tours;
using localtour.Transactions.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace localtour.Transactions
{

    public class TransactionAppService : localtourAppServiceBase, ITransactionAppService
    {
        private readonly IRepository<Transaction, int> _transactionRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRepository<Booking, int> _bookingRepository;
        private readonly IRepository<Tour, int> _tourRepository;

        public TransactionAppService(IRepository<Transaction, int> transactionRepository, IRepository<Booking, int> bookingRepository, IRepository<Tour, int> tourRepository, IWebHostEnvironment hostEnvironment)
        {
            _transactionRepository = transactionRepository;
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input)
        {
            var filteredTransactions = _transactionRepository.GetAll();

            var transactions = from o in filteredTransactions

                               join o1 in _bookingRepository.GetAll() on o.BookingId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _tourRepository.GetAll() on s1.TourId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new GetTransactionForViewDto()
                               {
                                   Transaction = new TransactionDto
                                   {
                                       Id = o.Id,
                                       TransactionDate = o.TransactionDate,
                                       Amount = o.Amount,
                                       BookingId = o.BookingId,
                                       CardNumber = o.CardNumber.Substring(o.CardNumber.Length - 4),
                                       NameOnCard = o.NameOnCard,
                                       Status = o.Status
                                   },
                                   TourName = s2.Name,
                                   BookingCode = "B-" + s1.Id
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

        protected virtual async Task Create(CreateOrEditTransactionDto input)
        {
            var transaction = ObjectMapper.Map<Transaction>(input);


            if (AbpSession.TenantId != null)
            {
                transaction.TenantId = (int?)AbpSession.TenantId;
            }


            await _transactionRepository.InsertAsync(transaction);
        }

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
