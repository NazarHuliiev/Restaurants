using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Services;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddSqlServer<RestaurantsDbContext>(connectionString, optionsAction: options => options.EnableSensitiveDataLogging());
        //services.AddDbContext<RestaurantsDbContext>();
        services
            .AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantsSeeder, RestaurantsSeeder>();
        
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasCurrentCountry, builder => builder.RequireClaim(AppClaimTypes.CurrentCountry))
            .AddPolicy(PolicyNames.AlLeast20, builder => builder.AddRequirements(new MinimalAgeRequirement(20)))
            .AddPolicy(PolicyNames.AlLeast2RestaurantsOwner, builder => builder.AddRequirements(new MinimalRestaurantsOwnerRequirement(2)));
            //.AddPolicy(PolicyNames.HasCurrentCountry, builder => builder.RequireClaim(AppClaimTypes.CurrentCountry, "Ukraine"));
        services.AddScoped<IAuthorizationHandler, MinimalAgeRequirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        services.AddScoped<IAuthorizationHandler, MinimalRestaurantsOwnerRequirementHandler>();

        services.AddScoped<IBlobStorageService, BlobStorageService>();

        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
    }
}