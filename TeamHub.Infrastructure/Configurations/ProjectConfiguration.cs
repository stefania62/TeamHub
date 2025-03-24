using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.Domain.Entities;

namespace TeamHub.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the EF Core mapping for the <see cref="Project"/> entity.
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Description)
            .HasMaxLength(100);

        // One-to-many: Project → TaskItem
        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: Project → ProjectEmployee
        builder.HasMany(p => p.Employees)
            .WithOne(pe => pe.Project)
            .HasForeignKey(pe => pe.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}