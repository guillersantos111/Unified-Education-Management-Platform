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
    public class StudentSubjectEnrollmentConfiguration : IEntityTypeConfiguration<StudentSubjectEnrollmentEntity>
    {

        public void Configure(EntityTypeBuilder<StudentSubjectEnrollmentEntity> builder)
        {
            builder
                .HasKey(sse => sse.StudentSubjectEnrollmentId);

            builder
                .HasOne(sse => sse.Students)
                .WithMany(s => s.StudentSubjectEnrollment)
                .HasForeignKey(sse => sse.StudentId);

            builder
                .HasOne(sse => sse.Subjects)
                .WithMany(sb => sb.StudentSubjectEnrollment)
                .HasForeignKey(sse => sse.SubjectId);
        }
    }
}
