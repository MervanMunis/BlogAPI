using System;
using BlogAPI.Auth.Models;
using BlogAPI.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Auth.Contollers
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add the "Author" role
            if (await roleManager.FindByNameAsync("Author") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Author"));
            }

            // Add the "Admin" role
            if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            // Example admin user
            string adminEmail = "admin@admin.com";
            string password = "Admin1234!";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                }
            }
        }
    }
}