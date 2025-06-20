using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HostelProperty.DataAccess.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(30);

        builder
            .HasMany(r => r.RoomSubjects)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId);

        builder.Property(c => c.Floor)
            .HasMaxLength(3);
    }
}