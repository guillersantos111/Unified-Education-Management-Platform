using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedEducationManagementSystem.Data_Connectivity.Configurations;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;


namespace UnifiedEducationManagementSystem.Data_Connectivity.Data
{
    public class UEMPDbContext : DbContext
    {
        public UEMPDbContext(DbContextOptions<UEMPDbContext> options) : base (options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles {  get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<AdminEntity> AdminEntity { get; set; }
        public DbSet<StudentsEntity> Students {  get; set; }
        public DbSet<StudentSubjectEnrollmentEntity> StudentSubjectEnrollment { get; set; }
        public DbSet<SubjectandSchedulesEntity> SubjectandSchedules { get; set; }
        public DbSet<CoursesEntity> Courses { get; set; }
        public DbSet<RequestsEntity> Requests { get; set; }
        public DbSet<ActivityLogsEntity> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ActivityLogsConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new StudentSubjectEnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectandSchedulesConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
