using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostelProperty.DataAccess.Repositories
{
    public class ResidentRepository
    {
        private readonly MyDbContext myDbContext;

        public ResidentRepository(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        public async Task<List<Resident>> GetAll()
        {
            return await myDbContext.Residents
                .AsNoTracking()
                .Include(c => c.Room)
                .ToListAsync();
        }

        public async Task<Resident?> GetById(Guid id)
        {
            return await myDbContext.Residents
                .AsNoTracking()
                .Include(c => c.Subjects)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Resident>?> GetByFIO(string searchText)
        {
            return await myDbContext.Residents
                .AsNoTracking()
                .Where(c => 
                    c.FirstName.Contains(searchText) ||
                    c.MiddleName.Contains(searchText) ||
                    c.LastName.Contains(searchText))
                .ToListAsync();
        }

        public async Task Add(
            string firstName,
            string middleName,
            string lastName,
            byte age,
            byte numberCourse,
            Guid? roomId)
        {
            var searchedRoom = myDbContext.Rooms.FirstOrDefault(c => c.Id == roomId);

            if (searchedRoom == null) {
                throw new Exception($"Room number {roomId} isn't exist.");
            }

            var resident = new Resident
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Age = age,
                NumberCourse = numberCourse,
                RoomId = roomId
            };

            await myDbContext.Residents.AddAsync(resident);
            await myDbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, Resident resident)
        {
            var response = await myDbContext.Residents
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FirstName, resident.FirstName)
                    .SetProperty(c => c.MiddleName, resident.MiddleName)
                    .SetProperty(c => c.LastName, resident.LastName)
                    .SetProperty(c => c.Age, resident.Age)
                    .SetProperty(c => c.NumberCourse, resident.NumberCourse)
                    .SetProperty(c => c.RoomId, resident.RoomId));

            if (response == 0)
            {
                throw new Exception("Resident not found");
            }

            await myDbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, List<Subject> subjects)
        {
            await myDbContext.Residents
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(s => 
                    s.SetProperty(c => c.Subjects, subjects));
        }

        public async Task Delete(Guid id)
        {
            await myDbContext.Residents.Where(r => r.Id == id).ExecuteDeleteAsync();
        }
    }
}
