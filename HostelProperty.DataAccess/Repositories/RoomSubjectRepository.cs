using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostelProperty.DataAccess.Repositories;

public class RoomSubjectRepository
{
    private readonly MyDbContext myDbContext;

    public RoomSubjectRepository(MyDbContext myDbContext)
    {
        this.myDbContext = myDbContext;
    }

    public async Task<List<RoomSubject>> GetAll()
    {
        return await myDbContext.RoomSubjects
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<RoomSubject>?> GetById(string id)
    {
        return await myDbContext.RoomSubjects
            .AsNoTracking()
            .Where(r => r.Id.ToString().StartsWith(id)) 
            .ToListAsync();
    }

    public async Task<List<RoomSubject>?> GetByTitle(string title)
    {
        return await myDbContext.RoomSubjects
            .AsNoTracking()
            .Where(r => r.Title.StartsWith(title))
            .ToListAsync();
    }

    public async Task Update(Guid id, RoomSubject roomSubject)
    {
        var currentDateTime = DateTime.Now;

        var response = await myDbContext.RoomSubjects
            .Where(r => r.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Title, roomSubject.Title)
                .SetProperty(c => c.DateReseption, roomSubject.DateReseption)
                .SetProperty(c => c.RoomId, roomSubject.RoomId)
                .SetProperty(c => c.DateReseption, currentDateTime));

        if (response == 0)
        {
            throw new Exception("Resident not found");
        }

        await myDbContext.SaveChangesAsync();
    }

    public async Task Add(RoomSubject roomSubject)
    {
        var searchedRoom = myDbContext.Rooms.FirstOrDefault(c => c.Id == roomSubject.RoomId);

        if (searchedRoom == null)
        {
            throw new Exception($"Room number {roomSubject.RoomId} isn't exist.");
        }

        await myDbContext.RoomSubjects.AddAsync(roomSubject);
        await myDbContext.SaveChangesAsync();
    }
}
