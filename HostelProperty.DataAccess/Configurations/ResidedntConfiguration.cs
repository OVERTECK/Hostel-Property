using HostelProperty.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HostelProperty.DataAccess.Configurations;

public class ResidedntConfiguration : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(r => r.Subjects)
            .WithOne(r => r.Resident)
            .HasForeignKey(r => r.ResidentId);

        builder
            .HasOne(r => r.Room)
            .WithMany(r => r.Residents)
            .HasForeignKey(r => r.RoomId);
    }
}
