using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.UserId);

        builder.HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<UserProfile>(p => p.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}