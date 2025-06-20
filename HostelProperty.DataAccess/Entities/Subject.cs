using System.ComponentModel.DataAnnotations;

namespace HostelProperty.DataAccess.Entities;

public class Subject
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    public DateTime? DateReseption { get; set; }

    public Guid? ResidentId { get; set; }
    
    public Resident? Resident { get; set; }
}
