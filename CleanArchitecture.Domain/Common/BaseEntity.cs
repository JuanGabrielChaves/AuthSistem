namespace CleanArchitecture.Domain.Entities.Common;

public abstract class BaseEntity : IAuditableEntity
{
    // Usaremos nombres estándar de la industria (Utc) para evitar líos de horarios
    public DateTime CreatedOnUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public string? ModifiedBy { get; set; }
}