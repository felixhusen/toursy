using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace localtour.Controllers
{
    public abstract class localtourControllerBase: AbpController
    {
        protected localtourControllerBase()
        {
            LocalizationSourceName = localtourConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
