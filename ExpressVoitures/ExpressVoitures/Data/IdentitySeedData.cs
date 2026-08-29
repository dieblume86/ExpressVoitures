using Microsoft.AspNetCore.Identity;

namespace ExpressVoitures.Data
{
    public static class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string AdminPassword = "P@ssword123";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(AdminUser);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = AdminUser,
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user, AdminPassword);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Fail to create user admin : {errors}");
                }
            }
            else
            {
                // Confirmed email if not already confirmed
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                }

                // Reset the password to the default admin password
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await userManager.ResetPasswordAsync(user, token, AdminPassword);
                if (!resetResult.Succeeded)
                {
                    var errors = string.Join("; ", resetResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"fail to reset admin password : {errors}");
                }
            }
        }
    }
}