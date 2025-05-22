using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HostelProperty.DataAccess.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Number);

        builder.
            HasMany(r => r.Residents)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId);

        builder.
            HasMany(r => r.RoomSubjects)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomNumber);
    }
}