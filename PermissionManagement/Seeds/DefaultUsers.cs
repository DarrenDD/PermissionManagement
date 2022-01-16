using Microsoft.AspNetCore.Identity;
using PermissionManagement.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PermissionManagement.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed Defualt Users
            var defualtUser = new IdentityUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defualtUser.Id)){
                var user = await userManager.FindByEmailAsync(defualtUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defualtUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defualtUser, Roles.Basic.ToString());
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed Defualt Users
            var defualtUser = new IdentityUser
            {
                UserName = "SuperAdmin@gmail.com",
                Email = "SuperAdmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defualtUser.Id)){
                var user = await userManager.FindByEmailAsync(defualtUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defualtUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defualtUser, Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defualtUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defualtUser, Roles.Basic.ToString());

                }
                await roleManager.SeedClaimsForSuperAdmin();
            } 
        }

        public async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole,"Products");
        }
        public async static Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach(var permission in allPermissions)
            {
                if(!allClaims.Any(a=>a.Type=="Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }


    }
}
