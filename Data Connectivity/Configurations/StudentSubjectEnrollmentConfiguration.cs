using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Configurations
{
    public class StudentSubjectEnrollmentConfiguration : IEntityTypeConfiguration<StudentSubjectEnrollmentEntity>
    {
        public void Configure(EntityTypeBuilder<StudentSubjectEnrollmentEntity> builder)
        {
            // Define the primary key for the StudentSubjectEnrollmentEntity
            builder
                .HasKey(sse => sse.StudentSubjectEnrollmentId);

            // Configure the relationship between StudentSubjectEnrollment and Students
            builder
                .HasOne(sse => sse.Students) // Each enrollment is associated with one student
                .WithMany(s => s.StudentSubjectEnrollmentEntities) // A student can have many enrollments
                .HasForeignKey(sse => sse.StudentId) // Foreign key in enrollment pointing to StudentId
                .OnDelete(DeleteBehavior.Cascade); // Delete enrollments when a student is deleted

            builder
                .HasOne(sse => sse.Subjects)
                .WithMany(sb => sb.StudentSubjectEnrollmentEntities) 
                .HasForeignKey(sse => sse.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(sse => sse.Courses)
                .WithMany(c => c.StudentSubjectEnrollmentEntities)
                .HasForeignKey(sse => sse.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
