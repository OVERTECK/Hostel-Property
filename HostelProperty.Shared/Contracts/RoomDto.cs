using HostelProperty.DataAccess.Entities;

namespace HostelProperty.Shared.Contracts;

public record class RoomDto(
    Guid Id,
    string Title,
    byte? CountResidents,
    byte? Floor,
    List<Resident> Residents,
    List<RoomSubject> RoomSubjects);
