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
    public class AssignmentConfiguration : IEntityTypeConfiguration<AssignmentEntity>
    {

        public void Configure(EntityTypeBuilder<AssignmentEntity> builder)
        {
            builder
                .HasKey(a => a.AssignmentId);
        }
    }
}
