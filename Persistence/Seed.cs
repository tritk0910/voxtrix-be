using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<AppUser>
            {
                new() {
                    DisplayName = "Tientien",
                    UserName = "tientien",
                    Email = "tiens2taeyeon@gmail.com"
                },
                new ()
                {
                    DisplayName = "khaitri074",
                    UserName = "khaitri074",
                    Email = "khaitri074@gmail.com"
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            await context.SaveChangesAsync();
        }
    }
}