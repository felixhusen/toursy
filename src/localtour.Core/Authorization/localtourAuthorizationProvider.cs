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
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            // Tours
            context.CreatePermission(PermissionNames.Pages_Tour_Create, L("CreateTour"));
            context.CreatePermission(PermissionNames.Pages_Tour_Approve, L("ApproveTour"));
            context.CreatePermission(PermissionNames.Pages_Tour_View, L("ViewTour"));
            context.CreatePermission(PermissionNames.Pages_Tour_Edit, L("EditTour"));
            context.CreatePermission(PermissionNames.Pages_Tour_Delete, L("DeleteTour"));
            // Bookings
            context.CreatePermission(PermissionNames.Pages_Booking_ViewAll, L("ViewAllBooking"));
            context.CreatePermission(PermissionNames.Pages_Booking_View, L("ViewBooking"));
            context.CreatePermission(PermissionNames.Pages_Booking_Edit, L("EditBooking"));
            context.CreatePermission(PermissionNames.Pages_Booking_Delete, L("DeleteBooking"));

            // Reviews
            context.CreatePermission(PermissionNames.Pages_Review_Edit, L("EditReview"));
            context.CreatePermission(PermissionNames.Pages_Review_Delete, L("DeleteReview"));
            context.CreatePermission(PermissionNames.Pages_Review_View, L("ViewReview"));
            context.CreatePermission(PermissionNames.Pages_Review_ViewAll, L("ViewAllReview"));

            // Transactions
            context.CreatePermission(PermissionNames.Pages_Transaction_Edit, L("EditTransaction"));
            context.CreatePermission(PermissionNames.Pages_Transaction_Delete, L("DeleteTransaction"));
            context.CreatePermission(PermissionNames.Pages_Transaction_View, L("ViewTransaction"));
            context.CreatePermission(PermissionNames.Pages_Transaction_ViewAll, L("ViewAllTransaction"));

            // Disputes
            context.CreatePermission(PermissionNames.Pages_Dispute_Edit, L("EditDispute"));
            context.CreatePermission(PermissionNames.Pages_Dispute_Delete, L("DeleteDispute"));
            context.CreatePermission(PermissionNames.Pages_Dispute_View, L("ViewDispute"));
            context.CreatePermission(PermissionNames.Pages_Dispute_ViewAll, L("ViewAllDispute"));

            // Requests
            context.CreatePermission(PermissionNames.Pages_Request_Edit, L("EditRequest"));
            context.CreatePermission(PermissionNames.Pages_Request_Delete, L("DeleteRequest"));
            context.CreatePermission(PermissionNames.Pages_Request_View, L("ViewRequest"));
            context.CreatePermission(PermissionNames.Pages_Request_ViewAll, L("ViewAllRequest"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, localtourConsts.LocalizationSourceName);
        }
    }
}
