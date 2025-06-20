using System.ComponentModel.DataAnnotations;

namespace HostelProperty.DataAccess.Entities;

public class User
{
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Password { get; set; }
}
