using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Infrastructure.Persistence;

public class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }
    
    public DbSet<Dish> Dishes { get; set; }
    
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
            .OwnsOne(r => r.Address);
        
        modelBuilder.Entity<Restaurant>()
            .OwnsOne(r => r.ContactInformation);

        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.OwnedRestaurants)
            .WithOne(r => r.Owner)
            .HasForeignKey(r => r.OwnerId);

        modelBuilder.Entity<ShoppingCartItem>()
            .HasOne(s => s.Dish)
            .WithMany()
            .HasForeignKey(s => s.DishId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}