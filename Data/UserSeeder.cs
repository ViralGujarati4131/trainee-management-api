using Microsoft.EntityFrameworkCore;
using Users.Models;
using PasswordHasher.Service.Interface;

public static class UserSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IPasswordHasherService passwordHash)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        string defaultUser = "admin";
        if (!await db.Users.AnyAsync(u => u.Username == defaultUser))
        {
            var adminUser = new User
            {
                Username = defaultUser,
                PasswordHash = passwordHash.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            db.Users.Add(adminUser);
            await db.SaveChangesAsync();
            System.Console.WriteLine("Default user created");
        }
        else
        {
            System.Console.WriteLine("Default user is not created");
        }
    }
}