using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using localtour.Configuration.Dto;

namespace localtour.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : localtourAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
