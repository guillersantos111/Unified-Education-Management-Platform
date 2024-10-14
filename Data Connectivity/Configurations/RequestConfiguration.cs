using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<RequestsEntity>
    {
        public void Configure(EntityTypeBuilder<RequestsEntity> builder)
        {
            builder.HasKey(rc => rc.RequestId);
            builder.Property(rc => rc.RequestId)
                .ValueGeneratedOnAdd();

            builder.Property(rc => rc.RequestDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(rc => rc.Students)
                .WithMany(s => s.RequestCertificates)
                .HasForeignKey(rc => rc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
