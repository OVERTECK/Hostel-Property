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
            int age,
            int numberCourse,
            int roomNumber)
        {
            var searchedRoom = myDbContext.Rooms.FirstOrDefault(c => c.Number == roomNumber);

            if (searchedRoom == null) {
                throw new Exception($"Room number {roomNumber} isn't exist.");
            }

            Room room = searchedRoom;

            var resident = new Resident
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Age = age,
                NumberCourse = numberCourse,
                RoomNumber = room.Number,
                Room = room
            };

            await myDbContext.Residents.AddAsync(resident);
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
