using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public class RestaurantsSeeder(RestaurantsDbContext dbContext) : IRestaurantsSeeder
{
    public async Task Seed()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
        
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                await dbContext.Restaurants.AddRangeAsync(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                await dbContext.Roles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        return 
        [
            new IdentityRole(UserRoles.Owner) { NormalizedName = UserRoles.Owner.ToUpper() },
            new IdentityRole(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpper() },
            new IdentityRole(UserRoles.User) { NormalizedName = UserRoles.User.ToUpper() }
        ];
    }
    
    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactInformation = new ContactInformation
                {
                    MainEmail = "contact@kfc.com",
                    MainPhoneNumber = "0731276609"
                },
                HasDelivery = true,
                OwnerId = "c987ae84-8369-469f-8fd4-76506cc03410",
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new()
                {
                    City = "London",
                    Street = "Cork St",
                    Building = "5"
                }
            },
            new ()
            {
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactInformation = new ContactInformation
                {
                    MainEmail = "contact@mcdonald.com",
                    MainPhoneNumber = "0731270966",
                    ExtraPhoneNumbers = new List<string> { "0969916863", "0971076266" }
                },
                HasDelivery = true,
                OwnerId = "c987ae84-8369-469f-8fd4-76506cc03410",
                Address = new()
                {
                    City = "London",
                    Street = "Boots",
                    Building = "193"
                }
            }
        ];

        return restaurants;
    }
}