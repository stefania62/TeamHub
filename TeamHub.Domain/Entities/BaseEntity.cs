namespace TeamHub.Domain.Entities;

/// <summary>
/// Represents base audit properties for all entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the creation timestamp of the entity.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last updated timestamp of the entity.
    /// Nullable to support entities that have never been updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}