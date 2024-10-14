using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Configurations
{
    public class ActivityLogsConfiguration : IEntityTypeConfiguration<ActivityLogsEntity>
    {

        public void Configure(EntityTypeBuilder<ActivityLogsEntity> builder)
        {
            builder
                .HasKey(a => a.LogId);

            builder
                .HasOne(a => a.Students)
                .WithMany(s => s.ActivityLogs)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
