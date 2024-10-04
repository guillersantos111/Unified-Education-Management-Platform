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
    public class SubjectConfiguration : IEntityTypeConfiguration<SubjectsEntity>
    {

        public void Configure(EntityTypeBuilder<SubjectsEntity> builder)
        {
            builder.HasKey(sb => sb.SubjectId);
            builder
                .Property(s => s.DescriptiveTitle)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
