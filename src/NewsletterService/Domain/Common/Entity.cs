namespace Domain.Common;

public abstract class Entity<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedDate {  get; set; }
    public DateTime EditedDate { get; set; }
    public byte[] Version { get; set; } = [];
}
