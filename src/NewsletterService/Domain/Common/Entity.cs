namespace Domain.Common;

public abstract class Entity<TId> : AuditEntity
{
    public TId Id { get; set; } = default!;
}

public abstract class AuditEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime EditedDate { get; set; }
    public byte[] Version { get; set; } = [];
}
