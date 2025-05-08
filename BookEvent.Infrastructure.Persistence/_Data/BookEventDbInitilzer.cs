using BookEvent.Core.Domain.Contracts.Persestence.DbInitializers;
using BookEvent.Core.Domain.Entities._Identity;
using BookEvent.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Infrastructure.Persistence._Data
{
    public class BookEventDbInitilzer(BookEventDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IBookEventDbInitializer
    {

        public async Task InitializeAsync()
        {
            var pendingmigration = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingmigration.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }
        public async Task SeedAsync()
        {
            var roles = new[] { Roles.Admin, Roles.User };

            if (!dbContext.Roles.Any())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));

                }
            }

            if (!dbContext.Users.Any())
            {
                var user = new ApplicationUser
                {
                    FullName = "Mohamed Hamdy",
                    UserName = "Mohammedhamdi726@gmail.com",
                    Email = "Mohammedhamdi726@gmail.com",
                    PhoneNumber = "01029442023",
                };

                await userManager.CreateAsync(user, "01124833532");
                await userManager.AddToRoleAsync(user, Roles.Admin);


            }


        }
    }
}
