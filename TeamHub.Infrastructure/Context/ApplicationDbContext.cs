using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data.Configurations;

namespace TeamHub.Infrastructure.Data.Context;

/// <summary>
/// Database context for TeamHub API.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Creates a new instance of the ApplicationDbContext.
    /// </summary>
    /// <param name="options">The options used to configure the database context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new ProjectConfiguration());
        builder.ApplyConfiguration(new ProjectEmployeeConfiguration());
        builder.ApplyConfiguration(new TaskItemConfiguration());
    }
}
