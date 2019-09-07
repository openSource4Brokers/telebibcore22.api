using System.Collections.Generic;
using System.Linq;
using telebibcore22.api.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace telebibcore22.api.Data
{
    public class Seed
    {
        private const string usersPath = "Data/UserSeedData.json";
        private const string qualifiersPath = "Data/TbQualifierSeedData.json";
        private const string valeursPath = "Data/TbValeurSeedData.json";        

        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText(path: usersPath);
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"},
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.Photos.SingleOrDefault().IsApproved = true;
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "info@rv.be"
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "thequickbrownfox").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                }
            }
        }

        public static void SeedTbValeurs(DataContext context)
        {
            if (!context.TbValeurs.Any())
            {
                var valeursData = System.IO.File.ReadAllText(path: valeursPath);
                var valeurs = JsonConvert.DeserializeObject<List<TbValeur>>(valeursData);

                foreach (var valeur in valeurs)
                {
                    context.TbValeurs.Add(valeur);
                }
                context.SaveChanges();                
            }
        }

        public static void SeedTbQualifiers(DataContext context)
        {
            if (!context.TbQualifiers.Any())
            {
                var qualifiersData = System.IO.File.ReadAllText(path: qualifiersPath);
                var qualifiers = JsonConvert.DeserializeObject<List<TbQualifier>>(qualifiersData);

                foreach (var qualifier in qualifiers)
                {
                    context.TbQualifiers.Add(qualifier);
                }
                context.SaveChanges();                
            }
        }
    }
}