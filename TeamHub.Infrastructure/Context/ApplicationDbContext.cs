using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamHub.Domain.Entities;

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

        // Project-Employee Many-to-Many Relationship
        builder.Entity<ProjectEmployee>()
            .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

        builder.Entity<ProjectEmployee>()
            .HasOne(pe => pe.Project)
            .WithMany(p => p.Employees)
            .HasForeignKey(pe => pe.ProjectId);

        builder.Entity<ProjectEmployee>()
            .HasOne(pe => pe.Employee)
            .WithMany(e => e.Projects)
            .HasForeignKey(pe => pe.EmployeeId);

        // Task-Employee Relationship
        builder.Entity<TaskItem>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
