using Abp.Application.Services;
using localtour.MultiTenancy.Dto;

namespace localtour.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

