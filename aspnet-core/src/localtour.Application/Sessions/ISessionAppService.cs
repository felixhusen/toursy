using System.Threading.Tasks;
using Abp.Application.Services;
using localtour.Sessions.Dto;

namespace localtour.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
