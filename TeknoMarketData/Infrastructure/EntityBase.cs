using TeknoMarketData;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeknoMarketData.Infrastructure;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool Enabled { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public virtual User? User { get; set; }

    [NotMapped] public string UserName => User!.Name;
}

