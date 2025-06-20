using System;
using System.Collections.Generic;
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
            return await myDbContext.Residents.AsNoTracking().ToListAsync();
        }

        public async Task<Resident?> GetById(Guid id)
        {
            return await myDbContext.Residents.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Add(
            string firstName,
            string middleName,
            string lastName,
            byte age,
            byte numberCourse,
            Room room)
        {
            var searchedRoom = myDbContext.Rooms.FirstOrDefault(c => c.Id == room.Id);

            if (searchedRoom == null) {
                throw new Exception($"Room number {room.Id} isn't exist.");
            }

            var resident = new Resident
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Age = age,
                NumberCourse = numberCourse,
                Room = room
            };

            await myDbContext.Residents.AddAsync(resident);
            await myDbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, Resident resident)
        {
            await myDbContext.Residents
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.FirstName, resident.FirstName)
                    .SetProperty(c => c.MiddleName, resident.MiddleName)
                    .SetProperty(c => c.LastName, resident.LastName)
                    .SetProperty(c => c.Age, resident.Age)
                    .SetProperty(c => c.NumberCourse, resident.NumberCourse)
                    .SetProperty(c => c.Subjects, resident.Subjects));
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
