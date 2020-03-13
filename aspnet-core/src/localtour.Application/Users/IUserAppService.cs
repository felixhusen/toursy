using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using localtour.Roles.Dto;
using localtour.Users.Dto;

namespace localtour.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
