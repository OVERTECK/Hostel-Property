using System.ComponentModel.DataAnnotations;

namespace HostelProperty.DataAccess.Entities;

public class Room
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public byte? CountResidents { get; set; }

    public byte? Floor { get; set; }

    public List<Resident>? Residents { get; set; }

    public List<RoomSubject>? RoomSubjects { get; set; }
}