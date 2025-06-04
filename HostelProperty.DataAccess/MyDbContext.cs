using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using HostelProperty.DataAccess.Configurations;

namespace HostelProperty.DataAccess
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomSubject> RoomSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ResidedntConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
