using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace localtour.Authorization
{
    public class localtourAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tour_Create, L("Tours"));
            context.CreatePermission(PermissionNames.Pages_Tour_Edit, L("Tours"));
            context.CreatePermission(PermissionNames.Pages_Tour_Delete, L("Tours"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_User_Bookings, L("Bookings"));
            context.CreatePermission(PermissionNames.Pages_User_Transactions, L("Transactions"));
            context.CreatePermission(PermissionNames.Pages_User_Disputes, L("Disputes"));
            context.CreatePermission(PermissionNames.Pages_User_Requests, L("Requests"));
            context.CreatePermission(PermissionNames.Pages_User_Reviews, L("Reviews"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, localtourConsts.LocalizationSourceName);
        }
    }
}
