using EduAuth.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduAuth.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
