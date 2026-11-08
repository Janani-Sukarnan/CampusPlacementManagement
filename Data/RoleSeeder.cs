
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Create application roles
            string[] roles =
            {
                RoleConstants.SuperAdmin,
                RoleConstants.PlacementOfficer,
                RoleConstants.CompanyHR,
                RoleConstants.Student
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            // 2. Read SuperAdmin credentials from User Secrets
            var email = configuration["SuperAdmin:Email"];
            var password = configuration["SuperAdmin:Password"];
            var fullName = configuration["SuperAdmin:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                throw new InvalidOperationException(
                    "SuperAdmin configuration is missing.");
            }

            // 3. Check whether SuperAdmin already exists
            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    AccountStatus = AccountStatus.Approved,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    admin, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));
                }
            }

            // 4. Ensure the account is approved
            if (admin.AccountStatus != AccountStatus.Approved)
            {
                admin.AccountStatus = AccountStatus.Approved;
                admin.UpdatedAt = DateTime.UtcNow;

                var updateResult = await userManager.UpdateAsync(admin);

                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not approve the SuperAdmin account.");
                }
            }

            // 5. Assign SuperAdmin role
            if (!await userManager.IsInRoleAsync(
                admin, RoleConstants.SuperAdmin))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    admin, RoleConstants.SuperAdmin);

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not assign SuperAdmin role.");
                }
            }
        }
    }
}
