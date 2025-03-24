using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamHub.Domain.Entities;

namespace TeamHub.Infrastructure.Data.Configurations;

/// <summary>
/// Configures the EF Core mapping for the <see cref="ApplicationUser"/> entity.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FullName)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(u => u.Projects)
              .WithOne(pe => pe.Employee)
              .HasForeignKey(pe => pe.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Tasks)
               .WithOne(t => t.AssignedTo)
               .HasForeignKey(t => t.AssignedToId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}