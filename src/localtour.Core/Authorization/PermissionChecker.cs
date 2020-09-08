using Abp.Authorization;
using localtour.Authorization.Roles;
using localtour.Authorization.Users;

namespace localtour.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
