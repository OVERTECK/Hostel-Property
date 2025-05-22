namespace HostelProperty.DataAccess.Entities;

public class Room
{
    public int Number { get; set; }

    public List<Resident>? Residents { get; set; }

    public List<RoomSubject>? RoomSubjects { get; set; }
}