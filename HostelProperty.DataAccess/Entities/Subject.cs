namespace HostelProperty.DataAccess.Entities;

public class Subject
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public DateOnly DateReseption { get; set; }

    public Guid ResidentId { get; set; }
    
    public Resident? Resident { get; set; }
}
