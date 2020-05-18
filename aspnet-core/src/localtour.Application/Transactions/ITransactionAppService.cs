using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Transactions.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace localtour.Transactions
{
    public interface ITransactionAppService : IApplicationService
    {
        Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input);

        Task<GetTransactionForViewDto> GetTransactionForView(int id);

        Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTransactionDto input);

        Task Delete(EntityDto input);
    }
}
