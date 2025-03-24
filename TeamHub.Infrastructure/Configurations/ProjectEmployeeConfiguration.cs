using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.Domain.Entities;

namespace TeamHub.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the EF Core mapping for the <see cref="ProjectEmployee"/> entity.
/// </summary>
public class ProjectEmployeeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
{
    public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        // Composite primary key
        builder.HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

        // Project relationship
        builder.HasOne(pe => pe.Project)
               .WithMany(p => p.Employees)
               .HasForeignKey(pe => pe.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Employee relationship
        builder.HasOne(pe => pe.Employee)
               .WithMany(e => e.Projects)
               .HasForeignKey(pe => pe.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
