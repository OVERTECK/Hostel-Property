using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HostelProperty.DataAccess.Configurations;

public class RoomSubjectConfiguration : IEntityTypeConfiguration<RoomSubject>
{
    public void Configure(EntityTypeBuilder<RoomSubject> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .HasOne(s => s.Room)
            .WithMany(s => s.RoomSubjects)
            .HasForeignKey(s => s.RoomId);
    }
}