using System.Threading.Tasks;
using localtour.Configuration.Dto;

namespace localtour.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
