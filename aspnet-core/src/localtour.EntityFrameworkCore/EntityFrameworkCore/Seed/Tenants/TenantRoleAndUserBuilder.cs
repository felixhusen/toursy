using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using localtour.Authorization;
using localtour.Authorization.Roles;
using localtour.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace localtour.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly localtourDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(localtourDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateAdminRoleAndUser()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new localtourAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }

        private void CreateTouristRoleAndUser()
        {
            // Tourist role
            var touristRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Tourist);
            if (touristRole == null)
            {
                touristRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Tourist, StaticRoleNames.Tenants.Tourist) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            // Grant permissions for tourist

            var touristDefaultPermissions = new List<string>() { "Pages.Tour.View", "Pages.Booking.View", "Pages.Transaction.View", "Pages.Dispute.View", "Pages.Review.View", "Pages.Request.View" };

            var grantedTouristPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == touristRole.Id)
                .Select(p => p.Name)
                .ToList();

            var touristPermissions = PermissionFinder
                .GetAllPermissions(new localtourAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && !grantedTouristPermissions.Contains(p.Name))
                .ToList();

            if (touristPermissions.Any())
            {
                var permissions = touristDefaultPermissions.Select(permissionName => new RolePermissionSetting
                {
                    TenantId = _tenantId,
                    Name = permissionName,
                    IsGranted = true,
                    RoleId = touristRole.Id
                });
                _context.Permissions.AddRange(permissions);
                _context.SaveChanges();
            }
        }

        private void CreateTourGuideRoleAndUser()
        {
            // Tour guide role
            var tourGuideRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.TourGuide);
            if (tourGuideRole == null)
            {
                tourGuideRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.TourGuide, StaticRoleNames.Tenants.TourGuide) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant permissions for tourist

            var tourGuideDefaultPermissions = new List<string>() { "Pages.Tour.Create", "Pages.Tour.Edit", "Pages.Tour.Delete", "Pages.Tour.View", "Pages.Tour.Approve", "Pages.Transaction.Create", "Pages.Transaction.Edit", "Pages.Transaction.Delete", "Pages.Review.Create", "Pages.Review.Edit", "Pages.Review.Delete", "Pages.Request.Create", "Pages.Request.Edit", "Pages.Request.Delete", "Pages.Dispute.Create", "Pages.Dispute.Edit", "Pages.Dispute.Delete", "Pages.Booking.Create", "Pages.Booking.Edit", "Pages.Booking.Delete", "Pages.Booking.View", "Pages.Booking.ViewAll", "Pages.Transaction.View", "Pages.Transaction.ViewAll", "Pages.Dispute.View", "Pages.Dispute.ViewAll", "Pages.Request.ViewAll", "Pages.Request.View", "Pages.Review.ViewAll", "Pages.Review.View" };

            var grantedTourGuidePermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == tourGuideRole.Id)
                .Select(p => p.Name)
                .ToList();

            var tourGuidePermissions = PermissionFinder
                .GetAllPermissions(new localtourAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && !grantedTourGuidePermissions.Contains(p.Name))
                .ToList();

            if (tourGuidePermissions.Any())
            {
                var permissions = tourGuideDefaultPermissions.Select(permissionName => new RolePermissionSetting
                {
                    TenantId = _tenantId,
                    Name = permissionName,
                    IsGranted = true,
                    RoleId = tourGuideRole.Id
                });
                _context.Permissions.AddRange(permissions);
                _context.SaveChanges();
            }
        }

        private void CreateRolesAndUsers()
        {
            CreateAdminRoleAndUser();

            CreateTouristRoleAndUser();

            CreateTourGuideRoleAndUser();

        }
    }
}
