using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;


namespace UnifiedEducationManagementSystem.Data_Connectivity.Data
{
    public class StudentConfiguration : IEntityTypeConfiguration<StudentsEntity>
    {

        public void Configure(EntityTypeBuilder<StudentsEntity> builder)
        {
            builder
                .HasKey(s => s.StudentId);
            
            builder
                .Property(s => s.StudentId)
                .ValueGeneratedOnAdd();

            builder
                .Property(s => s.Contact)
                .HasColumnType("BIGINT")
                .IsRequired();

            builder
                .HasIndex(s => s.Contact)
                .IsUnique();

            builder
                .HasOne(s => s.Courses)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(s => s.RequestCertificates)
                .WithOne(rc => rc.Students)
                .HasForeignKey(rc => rc.StudentId);
        }
    }
}
