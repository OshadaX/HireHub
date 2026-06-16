using HireHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<HireHub.Domain.Entities.Application> Applications { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}
