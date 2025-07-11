﻿using System.Diagnostics;
using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostelProperty.DataAccess.Repositories;

public class RoomRepository
{
    private readonly MyDbContext myDbContext;

    public RoomRepository(MyDbContext myDbContext)
    {
        this.myDbContext = myDbContext;
    }

    public async Task<List<Room>> GetAll()
    {
        return await myDbContext.Rooms
            .AsNoTracking()
            .Include(c => c.Residents)
            .Include(c => c.RoomSubjects)
            .ToListAsync();
    }

    public async Task<List<Room>?> GetByTitle(string title)
    {
        return await myDbContext.Rooms
            .AsNoTracking()
            .Where(c => c.Title.StartsWith(title))
            .Include(c => c.Residents)
            .Include(c => c.RoomSubjects)
            .ToListAsync();
    }

    public async Task<List<Room>?> GetByFloor(byte number)
    {
        return await myDbContext.Rooms
            .AsNoTracking()
            .Where(c => c.Floor == number)
            .Include(c => c.Residents)
            .Include(c => c.RoomSubjects)
            .ToListAsync();
    }

    public async Task<Room?> GetById(Guid id)
    {
        return await myDbContext.Rooms
            .AsNoTracking()
            .Include(c => c.Residents)
            .Include(c => c.RoomSubjects)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task Update(Guid id, Room room)
    {
        var result = await myDbContext.Rooms
            .Where(r => r.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Title, room.Title)
                .SetProperty(c => c.CountResidents, room.CountResidents)
                .SetProperty(c => c.Floor, room.Floor));
    }

    public async Task Update(Guid number, List<RoomSubject> roomSubjects, List<Resident> residents, byte floor)
    {
        await myDbContext.Rooms
            .Where(r => r.Id == number)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Floor, floor)
                .SetProperty(c => c.RoomSubjects, roomSubjects)
                .SetProperty(c => c.Residents, residents));
    }

    public async Task Delete(Guid number)
    {
        await myDbContext.Rooms.Where(r => r.Id == number).ExecuteDeleteAsync();
    }

    public async Task DeleteResidentFromRoom(Guid id)
    {
        var searchedResident = await myDbContext.Residents.FirstOrDefaultAsync(r => r.Id == id);

        if (searchedResident != null)
        {
            searchedResident.RoomId = null;

            myDbContext.SaveChanges();

        } else
        {
            throw new Exception("Not found resident");
        }
    }

    public async Task DeleteRoomSubjectFromRoom(Guid id)
    {
        var searchedRoomSubject = await myDbContext.RoomSubjects.FirstOrDefaultAsync(r => r.Id == id);

        if (searchedRoomSubject != null)
        {
            searchedRoomSubject.RoomId = null;

            myDbContext.SaveChanges();
        }
    }

    public async Task DeleteRoom(Guid id)
    {
        var searchedRoom = await myDbContext.Rooms
            .Include(c => c.Residents)
            .Include(c => c.RoomSubjects)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (searchedRoom == null)
            throw new Exception("Room not found");

        var residents = searchedRoom.Residents;

        if (residents != null)
        {
            foreach (var resident in residents)
            {
                resident.RoomId = null;
            }
        }

        var roomSubjects = searchedRoom.RoomSubjects;

        if (roomSubjects != null)
        {
            foreach (var roomSubject in roomSubjects)
            {
                roomSubject.RoomId = null;
            }
        }

        myDbContext.Rooms.Remove(searchedRoom);

        await myDbContext.SaveChangesAsync();
    }
}
