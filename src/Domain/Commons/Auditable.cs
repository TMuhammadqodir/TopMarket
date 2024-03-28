namespace Domain.Commons;

public class Auditable
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatetAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
