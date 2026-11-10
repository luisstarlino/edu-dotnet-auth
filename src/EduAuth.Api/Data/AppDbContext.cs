using EduAuth.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduAuth.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Module 03: one account per email — the real guarantee against duplicates.
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}
